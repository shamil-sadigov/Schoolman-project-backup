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
    public static partial class DependencyExtension
    {
        public static void AddPersistenceLayerForTesting(this IServiceCollection services)
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
               .AddEntityFrameworkStores<SchoolmanContext>()
               .AddDefaultTokenProviders();

            #endregion

            #region Database configuration

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddDbContext<SchoolmanContext>(ops =>
            {
                ops.UseMySql(configuration.GetConnectionString("LocalServer-MySql"));
            });

            #endregion
        }
    }
}