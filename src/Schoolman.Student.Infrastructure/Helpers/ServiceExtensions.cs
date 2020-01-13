using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure.AuthOptions;
using Schoolman.Student.Infrastructure.Helpers;
using Schoolman.Student.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Schoolman.Student.Infrastructure.Services
{
    public static class ServiceExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtFactory<AppUser>, JwtFactory>();

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

        public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService<ConfirmationEmailBuilder>, ConfirmationEmailService>();

            services.Configure<EmailOptions>("Confirmation", ops =>
                configuration.GetSection("EmailOptions:Yandex").Bind(ops));

            services.Configure<EmailTemplate>("Confirmation", template =>
            {
                string relativePath = configuration.GetSection("EmailOptions:Templates:Path:Confirmation").Value;

                var rootPath = services.BuildServiceProvider()
                                        .GetRequiredService<IHostingEnvironment>()
                                        .ContentRootPath;

                template.Path = Path.Combine(rootPath, relativePath);
            });
        }

        public static void AddUrlService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<UrlService>();

            // 👌🏽
            // this options is just for development mode
            services.Configure<UrlOptions>("Angular-AccountConfirmationUrl", urlOptions =>
                configuration.GetSection("UrlOptions:Angular-AccountConfirmationUrl").Bind(urlOptions));

            // for production 🤟
            // configures UrlOptions to be injected in urlService that
            // is going to build account confirmation url for email service
            services.Configure<UrlOptions>("Aspnet-AccountConfirmationUrl", urlOptions =>
            {
                var httpContext = services.BuildServiceProvider()
                                          .GetRequiredService<IHttpContextAccessor>()
                                          .HttpContext;

                urlOptions.Scheme = httpContext.Request.Scheme;
                urlOptions.Host = httpContext.Request.Host.Host;
                urlOptions.Path = configuration.GetSection("UrlOptions:Aspnet-AccountConfirmationUrl.Path")
                                               .Value;

                if (httpContext.Request.Host.Port.HasValue)
                    urlOptions.Port = httpContext.Request.Host.Port.Value;
            });
        }
    }
}
