using Application.Common.Models;
using Domain;
using Domain.Models;
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

        public Claim[] BuildClaims(Customer customer)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, customer.User.Email),
                    new Claim(AppClaimTypes.CustomerId, customer.Id),
                    new Claim(AppClaimTypes.Role, customer.Role?.Name),
                    new Claim(AppClaimTypes.Company, customer.Company?.Name),
                    new Claim(AppClaimTypes.CompanyId, customer.Company?.Id),
            };

            return claims;
        }

        public string GetCustomerFromClaims(IEnumerable<Claim> claims)
            => claims.FirstOrDefault(c => c.Type == AppClaimTypes.CustomerId).Value;
    }
}
