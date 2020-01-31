using Domain;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class JwtClaimsBuilder : ITokenClaimsBuilder
    {
        public Claim[] Build(User user)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            return claims;

        }
    }
}
