//#define Server_Local

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure.Services;

namespace Schoolman.Student.Infrastructure
{
    public static class DependencyExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(ops =>
            {
                ops.User.RequireUniqueEmail = true;
                ops.Password.RequireUppercase = true;
                ops.Password.RequiredLength = 8;
                ops.SignIn.RequireConfirmedEmail = true;
                ops.SignIn.RequireConfirmedPhoneNumber = false;
                ops.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<UserDataContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<UserDataContext>(ops =>
            {
#if DEBUG
#if Server_Local
                ops.UseMySql(configuration.GetConnectionString("LocalServer"));
#else

                ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
                //ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
#endif
#elif RELEASE
                ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
#endif
            });

            services.AddJwtAuthentication(configuration);
            services.AddEmailService(configuration);
            services.AddUrlService(configuration);

            services.AddScoped<IUserService<User>, UserService>();
            services.AddScoped<IAuthService<User>, AuthService>(); 
            services.AddHttpContextAccessor();
        }
    }
}