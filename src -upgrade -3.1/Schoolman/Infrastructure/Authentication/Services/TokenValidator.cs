using Application.Services;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class TokenValidator : ITokenValidator<TokenValidationParameters>
    {
        public Result<Claim[]> ValidateAccessToken(string accessToken, TokenValidationParameters validationParameters)
        {
            validationParameters.ValidateLifetime = false;
            // false because we don't need to validate token lifetime
            // if lifetime is true and token's valid time is expired

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(accessToken))
                return Result<Claim[]>.Failure("Access token is not valid");

            try
            {
                var principal = jwtHandler.ValidateToken(token: accessToken,
                    validationParameters: validationParameters, out SecurityToken token);

                if (principal == null)
                    return Result<Claim[]>.Failure("Access token is not valid");

                return Result<Claim[]>.Success(principal.Claims.ToArray());
            }
            catch (ArgumentException)
            {
                return Result<Claim[]>.Failure("Access token is invalid");
            }
        }

        public Result ValidateRefreshToken(RefreshToken refreshToken, string accessTokenId)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (refreshToken.Token == null)
                return AuthResult.Failure("Refresh token doesn't exist");

            if (refreshToken.ExpirationTime < currentTime)
                return AuthResult.Failure("Refresh token has been expired");

            if (refreshToken.AccessTokenId != accessTokenId)
                return AuthResult.Failure("Refresh token doesn't match with Access token");

            return Result.Success();
        }
    }
}
