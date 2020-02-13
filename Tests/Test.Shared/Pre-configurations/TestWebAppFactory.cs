using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Test.Shared
{
    public class TestWebAppFactory : WebApplicationFactory<StartupForTesting>
    {
        protected override IHostBuilder CreateHostBuilder() =>
         Host.CreateDefaultBuilder()
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<StartupForTesting>();
             });
    }
}
