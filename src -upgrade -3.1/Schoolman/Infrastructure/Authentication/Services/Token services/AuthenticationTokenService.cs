using Application.Common.Models;
using Application.Services.Token;
using Application.Services.Token.Validators.Access_Token_Validator;
using Authentication.Options;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class AuthenticationTokenService : IAuthenticationTokenServiceRefined
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly IUserService userService;
        private readonly JwtOptions jwtOptions;
        private readonly RefreshTokenOptions refreshTokenOptions;

        public AuthenticationTokenService(IAccessTokenService accessTokenService, 
                                IRefreshTokenService refreshTokenService,
                                IUserService userService,
                                JwtOptions jwtOptions,
                                RefreshTokenOptions refreshTokenOptions)
        {
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshTokenService;
            this.userService = userService;
            this.jwtOptions = jwtOptions;
            this.refreshTokenOptions = refreshTokenOptions;
        }

        public async Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(User user)
        {
            #region Access token generation

            var jwtParameters = new AccessTokenCreationParameters(user, jwtOptions);

            Result<string> accessTokenGenerationResult = await accessTokenService.GenerateTokenAsync(jwtParameters);

            if (!accessTokenGenerationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(accessTokenGenerationResult.Errors);

            // retrieve access token
            string accessToken = accessTokenGenerationResult.Response;

            #endregion

            #region Refresh token generation

            var refTokenParameters = new RefreshTokenCreationParameters(user, refreshTokenOptions);

            Result<string> refreshTokenGenerationResult = 
                await refreshTokenService.GenerateTokenAsync(refTokenParameters);

            if (!refreshTokenGenerationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(refreshTokenGenerationResult.Errors);


            // retrieve refreshtoken to separate variable
            string refreshToken = refreshTokenGenerationResult.Response;

            #endregion

            var response = new AuthenticationTokens(accessToken, refreshToken);
            return Result<AuthenticationTokens>.Success(response);
        }


        public async Task<Result<AuthenticationTokens>> ExchangeAuthenticationTokensAsync(AuthenticationTokens tokens)
        {
            #region AccessToken Validation

            var accessTokenValParams = 
                new AccessTokenValidationParameters(tokens.AccessToken, (TokenValidationParameters)jwtOptions);

            Result<ClaimsPrincipal> accessTokenValidationResult = 
                await accessTokenService.ValidateTokenAsync(accessTokenValParams);

            if (!accessTokenValidationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(accessTokenValidationResult.Errors);

            ClaimsPrincipal tokenClaims = accessTokenValidationResult.Response;

            #endregion

            #region RefreshToken Validation

            var refreshTokenValidationParameters = 
                new RefreshTokenValidationParameters(tokens.RefresthToken);

            Result refTokenValResult = 
                await refreshTokenService.ValidateTokenAsync(refreshTokenValidationParameters);

            if (!refTokenValResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(refTokenValResult.Errors);

            #endregion

            #region Token Generation

            string userId = accessTokenService.GetUserIdFromClaims(tokenClaims);
            User user = await userService.FindAsync(userId);

            return await GenerateAuthenticationTokensAsync(user);
            #endregion
        }
    }
}
