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
    public class AuthTokenService : IAuthenticationTokenService2
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly IUserService userService;
        private readonly JwtOptions jwtOptions;

        public AuthTokenService(IAccessTokenService accessTokenService, 
                                IRefreshTokenService refreshTokenService,
                                IUserService userService,
                                JwtOptions jwtOptions)
        {
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshTokenService;
            this.userService = userService;
            this.jwtOptions = jwtOptions;
        }

        public async Task<Result<AuthenticationCredential>> GenerateAuthenticationTokensAsync(User user)
        {
            #region Access token generation

            var jwtParameters =
                new AccessTokenGenerationParameters(user, jwtOptions);

            Result<string> accessTokenGenerationResult =
                await accessTokenService.GenerateTokenAsync(jwtParameters);

            if (!accessTokenGenerationResult.Succeeded)
                return Result<AuthenticationCredential>.Failure(accessTokenGenerationResult.Errors);

            #endregion

            #region Refresh token generation

            Result<string> refreshTokenGenerationResult = 
                await refreshTokenService.GenerateTokenAsync(user);

            if (!refreshTokenGenerationResult.Succeeded)
                return Result<AuthenticationCredential>.Failure(refreshTokenGenerationResult.Errors);

            #endregion

            var response = new AuthenticationCredential
            (accessTokenGenerationResult.Response, refreshTokenGenerationResult.Response);
            return Result<AuthenticationCredential>.Success(response);
        }


        public async Task<Result<AuthenticationCredential>> ExchangeAuthenticationTokensAsync(AuthenticationCredential tokens)
        {
            #region AccessToken Validation

            var accessTokenValParams =
                new AccessTokenValidationParameters(tokens.AccessToken, (TokenValidationParameters)jwtOptions);

            Result<ClaimsPrincipal> accessTokenValidationResult = 
                await accessTokenService.ValidateTokenAsync(accessTokenValParams);

            if (!accessTokenValidationResult.Succeeded)
                return Result<AuthenticationCredential>.Failure(accessTokenValidationResult.Errors);

            #endregion

            #region RefreshToken Validation

            var refreshTokenValidationParameters = 
                new RefreshTokenValidationParameters(tokens.RefresthToken);

            Result refTokenValResult = 
                await refreshTokenService.ValidateTokenAsync(refreshTokenValidationParameters);

            if (refTokenValResult)
                return Result<AuthenticationCredential>.Failure(refTokenValResult.Errors);

            #endregion

            #region Token Generation

            ClaimsPrincipal tokenClaims = accessTokenValidationResult.Response;
            string userId = accessTokenService.GetUserIdFromClaims(tokenClaims);
            User user = await userService.GetById(userId);

            return await GenerateAuthenticationTokensAsync(user);
            #endregion
        }
    }
}
