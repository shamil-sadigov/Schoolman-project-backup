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
    public static class DependencyExtension
    {
        public static void AddAuthenticationLayer(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("authentication-settings.json");

            services.AddJwtAuthentication(configuration);
            services.AddScoped<IAuthService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenValidator<TokenValidationParameters>, TokenValidator>();
            services.AddScoped<ITokenClaimsBuilder, JwtClaimsBuilder>();

        }

        #region Private methods


        private static IConfiguration BuildConfiguration(string configurationFilePath) =>
                new ConfigurationBuilder().AddJsonFile(path: configurationFilePath,
                                                       optional: true,
                                                       reloadOnChange: true)
                                                       .Build();

        private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtOptions jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            services.AddSingleton(jwtOptions);

            services.AddAuthentication(ops =>
            {
                ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(ops =>
            {
                ops.SaveToken = true;           // explicit operator
                ops.TokenValidationParameters = (TokenValidationParameters)jwtOptions;
            });

            services.Configure<RefreshTokenOptions>(ops =>
                configuration.GetSection(nameof(RefreshTokenOptions)).Bind(ops));
        }


        #endregion

    }
}
