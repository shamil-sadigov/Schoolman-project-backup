using Application.Common.Models;
using Application.Services.Token;
using Application.Services.Token.Validators.Access_Token_Validator;
using Authentication.Options;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
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
    public class AuthTokenService : IAuthTokenService
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly IUserService userService;

        public AuthTokenService(IAccessTokenService accessTokenService, 
                                IRefreshTokenService refreshTokenService,
                                IUserService userService)
        {
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshTokenService;
            this.userService = userService;
        }

        public async Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(User user)
        {
            #region Access token generation

            Result<string> jwtCreationResult = await accessTokenService.GenerateTokenAsync(user);

            if (!jwtCreationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(jwtCreationResult.Errors);

            // retrieve access token
            string accessToken = jwtCreationResult.Response;

            #endregion

            #region Refresh token generation

            Result<string> refreshTokenGenerationResult = 
                await refreshTokenService.GenerateTokenAsync(user);

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

            Result<ClaimsPrincipal> accessTokenValidationResult = 
                await accessTokenService.ValidateTokenAsync(tokens.AccessToken);

            if (!accessTokenValidationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(accessTokenValidationResult.Errors);

            ClaimsPrincipal tokenClaims = accessTokenValidationResult.Response;

            #endregion

            #region RefreshToken Validation

            Result refTokenValResult = 
                await refreshTokenService.ValidateTokenAsync(tokens.RefresthToken);

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
