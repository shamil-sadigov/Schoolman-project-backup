using Application.Services;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer
{
    public class DbContextTest : TestBase
    {
        public DbContextTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) { }


        [Fact(DisplayName = "SchoolmanContext is registered in IoC")]
        public void SchoolmanContextNotNull()
        {
            var context = factory.Services.GetRequiredService<SchoolmanContext>();
            Assert.NotNull(context);
        }

    }
}
