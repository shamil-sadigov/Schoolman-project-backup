using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure.Data.Identity;
using Schoolman.Student.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{

    /// <summary>
    /// Manages JWT and Refresh token
    /// </summary>
    public class AuthTokenManager : IAuthTokenManager<AppUser>
    {
        private readonly JwtOptions jwtOptions;
        private readonly RefreshTokenOptions refreshTokenOptions;
        private readonly UserDataContext dataContext;
        private JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

        public AuthTokenManager(IOptionsMonitor<JwtOptions> jwtOptions,
                                IOptionsMonitor<RefreshTokenOptions> refreshTokenOptions,
                                UserDataContext dataContext)
        {
            this.jwtOptions = jwtOptions.CurrentValue;
            this.refreshTokenOptions = refreshTokenOptions.CurrentValue;
            this.dataContext = dataContext;
        }

        /// <summary>
        /// Generate JWT and Refresh token
        /// </summary>
        /// <param name="user">User for whom tokens will be created for</param>
        /// <returns>JWT and Refresh tokens</returns>
        public async Task<(string jwt, string refreshToken)> GenerateTokens(AppUser user)
        {
            // no validation yet

            var claims = GenerateClaims(user);
            var keyBytes = jwtOptions.SecretKey.GetBytes();

            (string jwt, string jwtId)  = GenerateJwt(claims, keyBytes);
            string refreshToken = await GenerateRefreshTokenAsync(jwtId, user.Id.ToString());

            return (jwt, refreshToken);
        }


        public Task<(Result result, string jwt, string refreshToken)> RefreshTokens(string jwt, string refreshToken)
        {






            throw new NotImplementedException();
        }








        #region Local methods

        /// <summary>
        /// Generate User-based claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<Claim> GenerateClaims(AppUser user)
        {
            var claims = new List<Claim>
               {
                    new Claim("UserId", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iss, jwtOptions.Issuer),
                    new Claim(JwtRegisteredClaimNames.Iat, jwtOptions.IssueDate.ToString())
               };
            return claims;
        }


        /// <summary>
        /// Generates JWT based on claims and secret keyword
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private (string jwt, string jwtId) GenerateJwt(List<Claim> claims, byte[] key)
        {
            var tokenDesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtOptions.ExpirationTime),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                IssuedAt = jwtOptions.IssueDate,
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key),
                                                      algorithm: SecurityAlgorithms.HmacSha256)
            };

            JwtSecurityToken securityToken = (JwtSecurityToken)tokenhandler.CreateToken(tokenDesciptor);

            string jwt = tokenhandler.WriteToken(securityToken);

            return (jwt, securityToken.Id);
        }

        private async Task<string> GenerateRefreshTokenAsync(string jwtId, string userId)
        {
            RefreshToken refreshToken = RefreshToken.NewRefreshToken(jwtId, userId, refreshTokenOptions);
            await dataContext.RefreshTokens.AddAsync(refreshToken);
            await dataContext.SaveChangesAsync();

            return refreshToken.Token.ToString();
        }



        #endregion



    }
}
