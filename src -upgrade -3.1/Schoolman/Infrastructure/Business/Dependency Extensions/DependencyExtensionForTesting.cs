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
    public static partial class DependencyExtension
    {
        public static void AddBusinessLayerForTesting(this IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration("business-settings.json");
            services.AddHttpContextAccessor();

            services.AddTransient<IConfirmationEmailService, ConfirmationEmailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<UrlService>();

            services.AddEmailOptionsForTesting(configuration);
            services.AddUrlOptions(configuration);
        }


        #region Private methods

        private static void AddEmailOptionsForTesting(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>("Confirmation", ops =>
                configuration.GetSection("EmailOptions:Yandex").Bind(ops));

            services.Configure<EmailTemplate>("Confirmation", template =>
            {
                string relativePath = configuration.GetSection("EmailOptions:Templates:Path:Confirmation").Value;

                var rootPath = services.BuildServiceProvider()
                                        .GetRequiredService<IWebHostEnvironment>()
                                        .ContentRootPath;

                template.Path = Path.Combine(rootPath, relativePath);
            });
        }

    }

    #endregion


}

