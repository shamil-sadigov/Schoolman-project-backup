using Application.Services;
using Authentication.Options;
using Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication
{
    public static partial class DependencyExtension
    {
        /// <summary>
        /// This extenstion method is indentended for Integration testing
        /// Yet we just add the same service as in above extension
        /// BUT
        /// Things can change, so you had better to use this extension rather that above one
        /// </summary>
        public static void AddAuthenticationLayerForTesting(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("authentication-settings.json");

            services.AddJwtAuthentication(configuration);
            services.AddScoped<IAuthService, AuthenticationServiceOLD>();
            services.AddScoped<IAuthTokenService, TokenService>();
            services.AddScoped<IAuthTokenValidator<TokenValidationParameters>, TokenValidator>();
            services.AddScoped<IAuthTokenClaimService, JwtClaimsBuilder>();
        }
    }
}
