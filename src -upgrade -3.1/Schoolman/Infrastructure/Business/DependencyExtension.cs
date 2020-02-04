using Business.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Authentication
{
    public static class DependencyExtension
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("business-settings.json");
            services.AddHttpContextAccessor();

            services.AddTransient<IConfirmationEmailService, ConfirmationEmailService>();
            services.AddTransient<IUserService, UserService>();
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
                                        .GetRequiredService<IHostingEnvironment>()
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
