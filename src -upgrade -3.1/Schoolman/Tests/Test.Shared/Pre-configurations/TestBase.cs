using Xunit;

namespace Test.Shared
{
    public class TestBase : IClassFixture<TestWebAppFactory>
    {
        protected readonly TestWebAppFactory factory;
        public TestBase(TestWebAppFactory testWebAppFactory)
        {
            this.factory = testWebAppFactory;
        }
    }
}
