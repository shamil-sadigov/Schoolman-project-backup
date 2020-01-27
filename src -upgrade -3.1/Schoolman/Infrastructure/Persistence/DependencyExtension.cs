using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public static class DependencyExtension
    {
        public static void AddPersistenceLayer(this IServiceCollection services)
        {
            #region Setting configuration

            IConfiguration configuration =
                new ConfigurationBuilder().AddEnvironmentVariables()
                                          .AddJsonFile(path: "persistence-settings.json",
                                                       optional: true,
                                                       reloadOnChange: true)
                                                       .Build();

            #endregion

            #region Identity configuration

            services.AddIdentity<User, Role>(ops =>
            {
                ops.User.RequireUniqueEmail = true;
                ops.Password.RequireUppercase = true;
                ops.Password.RequiredLength = 8;
                ops.SignIn.RequireConfirmedEmail = true;
                ops.SignIn.RequireConfirmedPhoneNumber = false;
                ops.Password.RequiredUniqueChars = 0;
            })
           .AddEntityFrameworkStores<DataContext>()
           .AddDefaultTokenProviders();

            #endregion

            #region Database configuration

            services.AddDbContext<DataContext>(ops =>
            {
                ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            #endregion
        }
    }
}