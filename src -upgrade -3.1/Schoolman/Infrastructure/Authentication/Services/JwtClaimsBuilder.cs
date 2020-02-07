using Domain;
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
        const string UserId = "UserId";

        public Claim[] BuildClaims(User user)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(UserId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            return claims;
        }

        public string GetUserIdFromClaims(IEnumerable<Claim> claims)
            => claims.FirstOrDefault(c => c.Type == UserId).Value;
    }
}
