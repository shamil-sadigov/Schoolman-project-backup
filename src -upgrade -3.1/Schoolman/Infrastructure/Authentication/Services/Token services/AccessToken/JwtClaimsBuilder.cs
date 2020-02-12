using Application.Common.Models;
using Domain;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class JwtClaimsBuilder : IAuthTokenClaimService
    {
        private readonly ILogger<JwtClaimsBuilder> logger;

        public JwtClaimsBuilder(ILogger<JwtClaimsBuilder> logger)
        {
            this.logger = logger;
        }

        public Claim[] BuildClaims(Customer customer)
        {
            if (customer != null && customer.UserInfo != null && customer.Role != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, customer.UserInfo.Email),
                    new Claim(AppClaimTypes.CustomerId, customer.Id),
                    new Claim(AppClaimTypes.Role, customer.Role?.Name ?? null),
                    new Claim(AppClaimTypes.Company, customer.Company?.Name ?? null),
                    new Claim(AppClaimTypes.CompanyId, customer.Company?.Id ?? null),
                 };

                return claims;
            }

            logger.LogError("JwtClaimsBuilder. Unable to build token claims since customer is invalid. " +
                            "Customer must not be null, and Customer.User + Customer.Role must not be null either {customer}", customer);

            throw new ArgumentNullException("Customer must not be valid for building claims");
        }

        public string GetCustomerFromClaims(IEnumerable<Claim> claims)
            => claims.FirstOrDefault(c => c.Type == AppClaimTypes.CustomerId).Value;
    }
}
