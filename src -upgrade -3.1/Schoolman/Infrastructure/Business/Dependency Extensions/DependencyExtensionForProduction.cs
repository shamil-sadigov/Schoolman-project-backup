using Application.Services;
using Application.Services.Business;
using Application.Services.Token.Validators.User_Token_Validator;
using Authentication.Services.EmailConfirmation;
using Business;
using Business.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Authentication
{
    public static partial class DependencyExtension
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("business-settings.json");
            services.AddTransient<IEmailSender<IConfirmationEmailBuilder>, ConfirmationEmailSender>();
            services.AddTransient<IConfirmationEmailService, ConfirmationEmailService>();
            services.AddTransient<ICurrentCustomerService, CurrentUserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddTransient<UrlService>();



            services.AddEmailOptions(configuration);
            services.AddUrlOptions(configuration);
        }

        #region Private methods


        private static IConfiguration BuildConfiguration(string configurationFilePath) =>
              new ConfigurationBuilder().AddJsonFile(path: configurationFilePath,
                                                     optional: true,
                                                     reloadOnChange: true)
                                                     .Build();

        private static void AddEmailOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>("Confirmation", ops =>
                configuration.GetSection("EmailOptions:SendInBlue").Bind(ops));

            services.Configure<EmailTemplate>("Confirmation", template =>
            {
                string relativePath = configuration.GetSection("EmailOptions:Templates:Path:Confirmation").Value;

                var rootPath = services.BuildServiceProvider()
                                        .GetRequiredService<IWebHostEnvironment>()
                                        .ContentRootPath;

                template.Path = Path.Combine(rootPath, relativePath);
            });
        }

        public static void AddUrlOptions(this IServiceCollection services, IConfiguration configuration)
        {
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
                urlOptions.Path = configuration.GetSection("UrlOptions:Aspnet-AccountConfirmationUrl:Path")
                                               .Value;

                if (httpContext.Request.Host.Port.HasValue)
                    urlOptions.Port = httpContext.Request.Host.Port.Value;
            });
        }

        #endregion
    }
}
