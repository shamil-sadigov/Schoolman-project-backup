using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.AuthOptions;
using Schoolman.Student.Infrastructure.Data.Identity;
using Schoolman.Student.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{

    /// <summary>
    /// Manages JWT and Refresh token
    /// </summary>
    public class JwtFactory : IJwtFactory<AppUser>
    {
        private readonly JwtOptions jwtOptions;
        private readonly RefreshTokenOptions refreshTokenOptions;
        private readonly UserDataContext dataContext;
        private readonly JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

        public JwtFactory(JwtOptions jwtOptions,
                          IOptionsMonitor<RefreshTokenOptions> refreshTokenOptions,
                          UserDataContext dataContext)
        {
            this.jwtOptions = jwtOptions;
            this.refreshTokenOptions = refreshTokenOptions.CurrentValue;
            this.dataContext = dataContext; 
        }

        /// <summary>
        /// Generate JWT and Refresh token
        /// </summary>
        /// <param name="user">User for whom tokens will be created for</param>
        /// <returns>JWT and Refresh tokens</returns>
        public async Task<AuthResult> GenerateTokensAsync(AppUser user)
        {
            // no validation yet
            var claims = GenerateClaims(user);
            var keyBytes = jwtOptions.SecretKey.GetBytes();
            
            (string jwt, string jwtId) = GenerateJwt(claims, keyBytes);
            string refreshToken = await GenerateRefreshTokenAsync(jwtId, user.Id.ToString());
            return AuthResult.Success(jwt, refreshToken);
        }


        public async Task<AuthResult> RefreshTokensAsync(string jwt, string refreshToken)
        {
            var (user, error) = jwt.GetUserFromToken((TokenValidationParameters)jwtOptions); // explicit operator

            if (user == null)
                return AuthResult.Failure(error);

            // time format in unix
            long token_time = user.Claims.GetTokenExpirationTime();
            long current_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (token_time > current_time)
                return AuthResult.Failure("JWT hasn't expired yet");

            string jti = user.Claims.GetJti();

            var storedRefreshToken = dataContext.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

            if (storedRefreshToken == null)
                return AuthResult.Failure("This refresh token doesn't exist");

            if (storedRefreshToken.Expiration_time < current_time)
                return AuthResult.Failure("Refresh token has expired");

            if (storedRefreshToken.Jti != jti)
                return AuthResult.Failure("Refresh token doesn't match with JWT");

            await dataContext.RemoveAndSaveAsync(storedRefreshToken);

            var (newJWT, newJTI) = GenerateJwt(user.Claims, jwtOptions.SecretKey.GetBytes());
            string newRefreshToken = await GenerateRefreshTokenAsync(newJTI, user.Claims.GetId());

            return AuthResult.Success(newJWT, newRefreshToken);
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iss, jwtOptions.Issuer)
            };

            return claims;
        }



        /// <summary>
        /// Generates JWT based on claims and secret keyword
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private (string jwt, string jwtId) GenerateJwt(IEnumerable<Claim> claims, byte[] key)
        {
            var tokenDesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtOptions.ExpirationTime),
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key),
                                                      algorithm: SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenhandler.CreateToken(tokenDesciptor) as JwtSecurityToken;
            string jwt = tokenhandler.WriteToken(securityToken);
            return (jwt, securityToken.Id);
        }



        /// <summary>
        /// Generates refresh token based on specified jti and userId and save it in database
        /// </summary>
        /// <param name="jti"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<string> GenerateRefreshTokenAsync(string newJti, string userId)
        {

            var refresh_token = dataContext.RefreshTokens.FirstOrDefault(u => u.UserId == userId);

            if (refresh_token != null)
                await dataContext.RemoveAndSaveAsync(refresh_token);


            var new_refresh_token = RefreshToken.NewRefreshToken(newJti, userId, refreshTokenOptions.ExpirationTime);
            await dataContext.AddAndSaveAsync(new_refresh_token);
            return new_refresh_token.Token;
        }

        #endregion
    }
}
