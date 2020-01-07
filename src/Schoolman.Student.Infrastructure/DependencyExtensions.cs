//#define Server_Local

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure.AuthOptions;
using Schoolman.Student.Infrastructure.Services;
using System.IO;
using ConfirmationEmailService = Schoolman.Student.Infrastructure.Services.ConfirmationEmailService;

namespace Schoolman.Student.Infrastructure
{
    public static class DependencyExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(ops =>
            {
                ops.User.RequireUniqueEmail = true;
                ops.Password.RequiredLength = 8;
                ops.SignIn.RequireConfirmedEmail = true;
                ops.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<UserDataContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<UserDataContext>(ops =>
            {
#if Server_Local
                ops.UseMySql(configuration.GetConnectionString("LocalServer"));
#else
                ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
#endif
            });

            // services
            services.AddTransient<IUserService<AppUser>, UserService>();
            services.AddScoped<IJwtFactory<AppUser>, JwtFactory>();
            services.AddScoped<IEmailService<ConfirmationEmailBuilder>, ConfirmationEmailService>();
            services.AddScoped<IAuthService, AuthService>();

            // configurations
            services.Configure<EmailOptions>("Confirmation", ops =>
                configuration.GetSection("EmailOptions:SendInBlue").Bind(ops));

            services.Configure<EmailTemplate>("Confirmation", template =>
            {
                    string relativePath = configuration.GetSection("EmailOptions:Templates:Path:Confirmation").Value;
                    var rootPath = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>()
                                                                 ?.ContentRootPath;
                    template.Path = Path.Combine(rootPath, relativePath);
            });

            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddSingleton(jwtOptions);

            services.Configure<RefreshTokenOptions>(ops =>
                configuration.GetSection(nameof(RefreshTokenOptions)).Bind(ops));


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

            services.AddHttpContextAccessor();
        }



    }


}