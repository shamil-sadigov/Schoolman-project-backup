using Application.Common.Models;
using Authentication.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Authentication.Options
{
    /// <summary>
    /// Jwt Options that should be configure in Startup.cs and appsettings.json
    /// </summary>
    public class JwtOptions: IAccessTokenOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }

        public static explicit operator TokenValidationParameters(JwtOptions ops)
        {
            return new TokenValidationParameters()

            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(ops.SecretKey.GetBytes()),
                ValidIssuer = ops.Issuer,
                ValidAudience = ops.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,

                // By Default ClockSkew is 5 minutes
                // This means that
                // if your token is expired, it will be valid still for 5 minutes
                // so set it to zero
                ClockSkew = TimeSpan.Zero
            };
        }
    }

}
