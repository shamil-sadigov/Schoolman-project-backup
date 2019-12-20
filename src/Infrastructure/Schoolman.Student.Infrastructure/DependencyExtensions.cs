#define Server_Local

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;

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
        }
    }
}