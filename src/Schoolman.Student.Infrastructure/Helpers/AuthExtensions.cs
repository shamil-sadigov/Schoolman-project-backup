using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Helpers
{
    internal static class AuthExtensions
    {
        /// <summary>
        /// Returns bytes of secret keywork in ASCI 
        /// </summary>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string securityKey)
            => Encoding.ASCII.GetBytes(securityKey);



        /// <summary>
        /// Get Unix time from Expiration claim. Throw exception if Expiration Claims has invalid format
        /// </summary>
        /// <param name="claims">User claims</param>
        /// <returns>Unix expiration time of JWT</returns>
        public static long GetTokenExpirationTime(this IEnumerable<Claim> claims)
        {
            var unixExpirationTime =
                claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.OrdinalIgnoreCase))
                .Value;

            return long.Parse(unixExpirationTime);

        }


        /// <summary>
        /// Return Jti of JWT token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GetJti(this IEnumerable<Claim> claims)
        {
            var jti =
                claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti, StringComparison.OrdinalIgnoreCase))
                .Value;

            return jti;
        }



        /// <summary>
        /// Gets ClaimsPrincipal from JWT token
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public static (ClaimsPrincipal user, string error) GetUserFromToken
              (this string jwt, TokenValidationParameters validationParameters)
        {
            // clone validationParameters since object is singleton-scoped
            var validationParams = validationParameters.Clone();

            // false because we need not to validate token lifetime
            // if lifetime is true and token's valid time is expired
            // then exception may be thrown in line 91

            validationParams.ValidateLifetime = false;
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(jwt))
                return (user: null, error: "JWT is not in correct format");

            try
            {
                var principal = jwtHandler.ValidateToken(token: jwt,
                                                  validationParameters: validationParams, out SecurityToken token);
                if (principal == null)
                    return (user: null, error: "JWT is not valid");

                return (user: principal, error: null);
            }
            catch (ArgumentException)
            {
                return (user: null, error: "Invalid access token");
            }
        }


        /// <summary>
        /// Simple method for merging addition and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<int> AddAndSaveAsync<T>(this DbContext db, T entity) where T : class
        {
            await db.Set<T>().AddAsync(entity);
            return await db.SaveChangesAsync();
        }


        /// <summary>
        /// Simple method for merthing removing and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<int> RemoveAndSaveAsync<T>(this DbContext db, T entity) where T : class
        {
            db.Set<T>().Remove(entity);
            return await db.SaveChangesAsync();
        }


        /// <summary>
        /// Get UserId from Claims
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GetId(this IEnumerable<Claim> claims)
        {
            var userId =
                claims.FirstOrDefault(c => c.Type.Equals("UserID", StringComparison.OrdinalIgnoreCase))
                .Value;

            return userId;
        }


        public static StringBuilder AddConfirmationUrl(this StringBuilder htmlTemplate, string url) =>
             htmlTemplate.Replace("aspnet-confirmation-url", url);


        public static StringBuilder AddUserName(this StringBuilder htmlTemplate, string token) =>
             htmlTemplate.Replace("aspnet-username", token);


        // ⛔️
        [Obsolete]
        /// <summary>
        /// Returns confirmation url based on webapi host
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string BuildConfirmationUrl(this UriBuilder builder, HttpRequest request, string userId, string token)
        {
            builder.Scheme = request.Scheme;
            builder.Host = request.Host.Host;
            builder.Path = "api/account/confirm";
            builder.Query = $"userId={userId}&confirmationToken={token}";
            if (request.Host.Port.HasValue)
                builder.Port = request.Host.Port.Value;

            return builder.Uri.ToString();
        }

        public static string BuildConfirmationUrl(this UrlService urlService, string userId, string token)=>
            urlService.BuildConfirmationUrlWithQuery($"userId={userId}&confirmationToken={token}");





        //public static string BuildConfirmationUrl(this UriBuilder builder, string userId, string token, ConfirmationUrlBuilder confirmationUrlBuilder)
        //{
        //    builder = confirmationUrlBuilder.BuildUrl(builder);
        //    builder.Query = $"userId={userId}&confirmationToken={token}";
        //    return builder.Uri.ToString();
        //}



    }
}
