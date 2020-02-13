using Application.Common.Models;
using Application.Services.Business;
using Application.Services.Token;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly IAccessTokenService accessTokenService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly ICustomerManager clientManager;

        public AuthTokenService(IAccessTokenService accessTokenService, 
                                IRefreshTokenService refreshTokenService,
                                ICustomerManager clientManager)
        {
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshTokenService;
            this.clientManager = clientManager;
        }

        public async Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(Customer client)
        {
            #region Access token generation

            Result<string> jwtCreationResult = await accessTokenService.GenerateTokenAsync(client);

            if (!jwtCreationResult.Succeeded)
                return Result<AuthenticationTokens>.Failure(jwtCreationResult.Errors);

            // retrieve access token
            string accessToken = jwtCreationResult.Response;

            #endregion

            #region Refresh token generation

            Result<string> refreshTokenGenerationResult = 
                await refreshTokenService.GenerateTokenAsync(client);

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

            string clientId = accessTokenService.GetCustomerIdFromClaims(tokenClaims);
            Customer client  = await clientManager.FindByIdAsync(clientId);
            return await GenerateAuthenticationTokensAsync(client);
            #endregion
        }
    }
}
