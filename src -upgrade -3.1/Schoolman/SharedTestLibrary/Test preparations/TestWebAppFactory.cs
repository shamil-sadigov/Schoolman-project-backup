using Common.IntegrationTest.Factory_Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.IntegrationTest.Factories
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
