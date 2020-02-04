using Application.Common.Models;
using Application.Services;
using Authentication.Helpers;
using Authentication.Options;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Services
{
    /// <summary>
    /// Manages JWT and Refresh token
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtOptions jwtOptions;
        private readonly IRepository<User> userRepository;
        private readonly ITokenClaimsBuilder claimsBuilder;
        private readonly ITokenValidator<TokenValidationParameters> validator;
        private readonly RefreshTokenOptions refreshTokenOptions;
        private readonly JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

        public TokenService(JwtOptions jwtOptions,
                            IOptionsMonitor<RefreshTokenOptions> refreshTokenOptions,
                            IRepository<User> userRepository,
                            ITokenClaimsBuilder claimsBuilder,
                            ITokenValidator<TokenValidationParameters> validator)
        {
            this.jwtOptions = jwtOptions;
            this.userRepository = userRepository;
            this.claimsBuilder = claimsBuilder;
            this.validator = validator;
            this.refreshTokenOptions = refreshTokenOptions.CurrentValue;
        }



        /// <summary>
        /// Generate JWT and Refresh token
        /// </summary>
        /// <param name="user">User for whom tokens will be created for</param>
        /// <returns>JWT and Refresh tokens</returns>
        public async Task<Result<AuthenticationCredentials>> GenerateTokensAsync(string userId)
        {
            var user = await userRepository.Collection.FindAsync(userId);

            var (jwt, refreshtoken) = _GenerateJwtAndRefreshTokens(user);

            await userRepository.SaveChangesAsync();

            var credentials = new AuthenticationCredentials();

            credentials.AccessToken = jwt;
            credentials.AccessToken = refreshtoken;


            return Result<AuthenticationCredentials>.Success(credentials);
        }


        public async Task<Result<AuthenticationCredentials>> RefreshTokensAsync(string accessToken, string refreshToken)
        {

            #region Validation section

            Result<Claim[]> validationResult = validator.ValidateAccessToken(accessToken, (TokenValidationParameters)jwtOptions);

            if (!validationResult.Succeeded)
                return Result<AuthenticationCredentials>.Failure(validationResult.Errors);

            User user = await userRepository.Collection.SingleOrDefaultAsync
                (usr => usr.RefreshToken.Token == refreshToken);

            if (user == null)
                return Result<AuthenticationCredentials>.Failure("Refresh token is not valid");


            Claim[] tokenClaims = validationResult.Response;

            string accessTokenId = tokenClaims.GetAccessTokenId();

            Result refTokenValidationResult = validator.ValidateRefreshToken(user.RefreshToken, accessTokenId);

            if (!refTokenValidationResult.Succeeded)
                return Result<AuthenticationCredentials>.Failure(refTokenValidationResult.Errors);

            #endregion


            #region Token generation section

            var (jwt, refreshtoken) = _GenerateJwtAndRefreshTokens(user);
            await userRepository.SaveChangesAsync();



            var credentials = new AuthenticationCredentials();
            credentials.AccessToken = jwt;
            credentials.AccessToken = refreshtoken;
            return Result<AuthenticationCredentials>.Success(credentials);

            #endregion
        }



        #region Local methods





        private (string jwt, string refreshToken) _GenerateJwtAndRefreshTokens(User user)
        {
            var claims = claimsBuilder.Build(user);

            (string jwt, string jti) = CreateAccessToken(claims);

            AddRefreshToken(user.RefreshToken, jti);

            return (jwt, user.RefreshToken.Token);


            #region Local methods of Locals methods 

            /// <summary>
            /// Generates JWT based on claims and secret keyword
            /// </summary>
            /// <param name="claims"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            (string accessToken, string accessTokenId) CreateAccessToken(Claim[] claims)
            {
                var key = jwtOptions.SecretKey.GetBytes();

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
            void AddRefreshToken(RefreshToken refreshToken, string jti)
            {
                refreshToken.Token = Guid.NewGuid().ToString();
                refreshToken.AccessTokenId = jti;
                refreshToken.IssueTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                refreshToken.ExpirationTime = DateTimeOffset.UtcNow.Add(refreshTokenOptions.ExpirationTime)
                                                                        .ToUnixTimeSeconds();
            }

            #endregion
        }



        #endregion
    }
}
