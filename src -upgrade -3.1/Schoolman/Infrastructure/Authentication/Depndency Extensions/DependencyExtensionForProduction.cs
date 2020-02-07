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
        /// This extension method is valid both for production and testing mode.
        /// If you take a look at other projects, you may find addition extension method for testing mode
        /// But in Authentication project its unnecessary
        /// </summary>
        public static void AddAuthenticationLayer(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("authentication-settings.json");

            services.AddJwtAuthentication(configuration);
            services.AddScoped<IAuthService, AuthenticationService>();
            services.AddScoped<IAuthTokenService, TokenService>();
            services.AddScoped<IAuthTokenValidator<TokenValidationParameters>, TokenValidator>();
            services.AddScoped<IAuthTokenClaimService, JwtClaimsBuilder>();
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
