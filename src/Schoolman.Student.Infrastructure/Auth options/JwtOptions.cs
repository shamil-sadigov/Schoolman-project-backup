using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Infrastructure.Helpers;
using System;

namespace Schoolman.Student.Infrastructure.AuthOptions
{
    /// <summary>
    /// Jwt Options that should be configure in Startup.cs and appsettings.json
    /// </summary>
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }


        public static explicit operator TokenValidationParameters(JwtOptions ops)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(ops.SecretKey.GetBytes()),
                ValidIssuer = ops.Issuer,
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };
        }
    }



}
