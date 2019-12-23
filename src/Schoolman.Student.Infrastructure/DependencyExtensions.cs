//#define Server_Local

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

            services.AddIdentity<AppUser, AppRole>(ops =>
            {
                ops.User.RequireUniqueEmail = true;
                ops.Password.RequiredLength = 8;
                ops.SignIn.RequireConfirmedEmail = true;
                ops.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<UserDataContext>();


            services.AddDbContext<UserDataContext>(ops =>
            {
#if Server_Local
                ops.UseMySql(configuration.GetConnectionString("LocalServer"));
#else
                ops.UseMySql(configuration.GetConnectionString("RemoteServer"));
#endif
            });

            services.AddScoped<IUserService, UserService>();




        }
    }
}