using Test.Shared.Preparations;
using Xunit;

namespace Test.Shared
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
