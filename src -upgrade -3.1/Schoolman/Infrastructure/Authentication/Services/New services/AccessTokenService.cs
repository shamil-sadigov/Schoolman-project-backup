using Application.Common.Models;
using Application.Services.Token;
using Application.Services.Token.Validators.Access_Token_Validator;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services.New_services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IAuthTokenClaimService claimsService;

        public AccessTokenService(IAuthTokenClaimService claimsBuilder)
        {
            claimsService = claimsBuilder;
        }

        public Task<Result<string>> GenerateTokenAsync(AccessTokenGenerationParameters parameters)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            User user = parameters.User;
            IAccessTokenOption accessTokenOption = parameters.AccessTokenOption;

            Claim[] claims = claimsBuilder.BuildClaims(user);
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(accessTokenOption.SecretKey);

            var tokenDesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(accessTokenOption.ExpirationTime),
                Audience = accessTokenOption.Audience,
                Issuer = accessTokenOption.Issuer,
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(secretKeyBytes),
                                                         algorithm: SecurityAlgorithms.HmacSha256)
            };

            SecurityToken securityToken = jwtTokenHandler.CreateToken(tokenDesciptor);
            string accessToken = jwtTokenHandler.WriteToken(securityToken);
            return Task.FromResult(Result<string>.Success(accessToken));
        }

        public string GetUserIdFromClaims(ClaimsPrincipal tokenClaims)
        {
            return claimsService.GetUserIdFromClaims(tokenClaims.Claims);
        }

        public async Task<Result<ClaimsPrincipal>> ValidateTokenAsync(AccessTokenValidationParameters accessTokenValidation)
        {
            accessTokenValidation.ValidationParameters.ValidateLifetime = false;

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(accessTokenValidation.AccessToken))
                return Result<ClaimsPrincipal>.Failure("Access token is not valid");

            try
            {
                var principal = jwtHandler.ValidateToken(token: accessTokenValidation.AccessToken,
                                         validationParameters: accessTokenValidation.ValidationParameters, out SecurityToken token);

                if (principal == null)
                    return Result<ClaimsPrincipal>.Failure("Access token is not valid");

                return Result<ClaimsPrincipal>.Success(principal);
            }
            catch (ArgumentException)
            {
                return Result<ClaimsPrincipal>.Failure("Access token is invalid");
            }

        }


    }
}
