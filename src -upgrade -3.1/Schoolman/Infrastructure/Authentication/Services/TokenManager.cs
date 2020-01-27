using Authentication.Helpers;
using Authentication.Options;
using Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    /// <summary>
    /// Manages JWT and Refresh token
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly JwtOptions jwtOptions;
        private readonly RefreshTokenOptions refreshTokenOptions;
        private readonly UserDataContext dataContext;
        private readonly JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

        public TokenManager(JwtOptions jwtOptions,
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
        public async Task<AuthResult> GenerateNewTokensAsync(User user)
        {
            // no validation yet
            var claims = GenerateClaims(user);
            var keyBytes = jwtOptions.SecretKey.GetBytes();

            (string jwt, string jwtId) = GenerateJwt(claims, keyBytes);
            string refreshToken = await GenerateRefreshTokenAsync(jwtId, user.Id);
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

            string jti = user.Claims.GetAccessTokenId();

            var storedRefreshToken = dataContext.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

            if (storedRefreshToken == null)
                return AuthResult.Failure("This refresh token doesn't exist");

            if (storedRefreshToken.Expiration_time < current_time)
                return AuthResult.Failure("Refresh token has expired");

            if (storedRefreshToken.Jti != jti)
                return AuthResult.Failure("Refresh token doesn't match with JWT");

            await dataContext.RemoveAndSaveAsync(storedRefreshToken);

            var (newJWT, newJTI) = GenerateJwt(user.Claims, jwtOptions.SecretKey.GetBytes());
            string newRefreshToken = await GenerateRefreshTokenAsync(newJTI, user.Claims.GetAccessTokenId());

            return AuthResult.Success(newJWT, newRefreshToken);
        }


        #region Local methods

        /// <summary>
        /// Generate User-based claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<Claim> GenerateClaims(User user)
        {
            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
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
                Issuer = jwtOptions.Issuer,
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
        private async Task<string> GenerateRefreshTokenAsync(string jti, string userId)
        {
            var refresh_token = dataContext.RefreshTokens.FirstOrDefault(u => u.UserId == userId);

            if (refresh_token != null)
                await dataContext.RemoveAndSaveAsync(refresh_token);

            var new_refToken = RefreshToken.NewRefreshToken(jti, userId, refreshTokenOptions.ExpirationTime);
            await dataContext.AddAndSaveAsync(new_refToken);
            return new_refToken.Token;
        }
        #endregion
    }
}
