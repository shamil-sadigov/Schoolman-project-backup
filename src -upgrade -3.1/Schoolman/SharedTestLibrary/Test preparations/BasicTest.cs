using Common.IntegrationTest.Factories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using Xunit;

namespace Common.IntegrationTest
{
    public class BasicTest:IClassFixture<TestWebAppFactory>
    {
        protected readonly TestWebAppFactory factory;

        public BasicTest(TestWebAppFactory  testWebAppFactory)
        {
            this.factory = testWebAppFactory;
        }
    }

}
