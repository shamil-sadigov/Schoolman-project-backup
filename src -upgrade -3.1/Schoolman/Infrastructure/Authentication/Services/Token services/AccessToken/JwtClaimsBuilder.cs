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

        public Claim[] BuildClaims(Client client)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, client.User.Email),
                    new Claim(AppClaimTypes.UserId, client.Id),
                    new Claim(AppClaimTypes.Role, client.Role?.Name),
                    new Claim(AppClaimTypes.Company, client.Company?.Name),
                    new Claim(AppClaimTypes.CompanyId, client.Company?.Id),
            };

            return claims;
        }

        public string GetUserIdFromClaims(IEnumerable<Claim> claims)
            => claims.FirstOrDefault(c => c.Type == AppClaimTypes.UserId).Value;
    }
}
