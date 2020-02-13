using Application.Common.Exceptions;
using Application.Services.Token;
using Authentication.Options;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services.New_services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IAuthTokenClaimService claimsService;
        private readonly ILogger<AccessTokenService> logger;
        private readonly JwtOptions jwtOptions;


        public AccessTokenService(IAuthTokenClaimService claimsBuilder,
                                  IOptionsMonitor<JwtOptions> jwtOptions,
                                  ILogger<AccessTokenService> logger)
        {
            claimsService = claimsBuilder;
            this.logger = logger;
            this.jwtOptions = jwtOptions.CurrentValue;
        }


        public string GetCustomerIdFromClaims(ClaimsPrincipal tokenClaims)
        {
            return claimsService.GetCustomerFromClaims(tokenClaims.Claims);
        }


        public Task<Result<string>> GenerateTokenAsync(Customer customer)
        {
            if(customer == null)
            {
                logger.LogError("AccessTokenService. Unable to generate token since customer parameter is null");
                throw new ArgumentNullException("Customer argument is null. Unable to generate claims for token");
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            Claim[] claims = claimsService.BuildClaims(customer);

            if (claims == null || claims.Length == 0)
            {
                logger.LogError("IAccessTokenService. Claims are not built. IAuthTokenClaimService return null while building claims for Customer {@customer}", customer);
                throw new ClaimsException("Claims are not built. IAuthTokenClaimService return null while building claims", customer);
            }

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

            var tokenDesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtOptions.ExpirationTime),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(secretKeyBytes),
                                                         algorithm: SecurityAlgorithms.HmacSha256)
            };

            SecurityToken securityToken = jwtTokenHandler.CreateToken(tokenDesciptor);
            string accessToken = jwtTokenHandler.WriteToken(securityToken);
            return Task.FromResult(Result<string>.Success(accessToken));
        }

      
        // todo: Add jwt time validation
        public async Task<Result<ClaimsPrincipal>> ValidateTokenAsync(string accessToken)
        {
            if (accessToken == null)
            {
                logger.LogError("AccessTokenService. Unable to validate token since token parameter is null");
            }

            // No worries. Just explicit conversion operator
            TokenValidationParameters validationParams = (TokenValidationParameters) jwtOptions;


            validationParams.ValidateLifetime = false;

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(accessToken))
                return Result<ClaimsPrincipal>.Failure("Access token is not valid");

            try
            {
                var principal = jwtHandler.ValidateToken(token: accessToken,
                                         validationParameters: validationParams, out SecurityToken token);



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
