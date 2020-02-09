using Domain;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.BusinessLayer
{
    public class UrlServiceTest : TestBase
    {
        private readonly UrlService urlService;
        public UrlServiceTest(TestWebAppFactory testWebAppFactory):base(testWebAppFactory)
        {
            urlService = factory.Services.GetRequiredService<UrlService>();
        }




        [Fact(DisplayName = "IUserService is registered in IoC and not null")]
        public void UserServiceIsRegisteredAndNotNull()
        {
            Assert.True(urlService != null, "IUserService is not registered");
        }


        [Fact(DisplayName = "UrlService.BuildConfirmationUrl with Spa url address returns relevant url")]
        public void UserService_Doesnt_Create_User_With_Emtpty_FirstName_And_Lastname()
        {
            // Don't test WebApiUrlAddress since it's address is build during runtime
            // urlService.UseWebapiUrlAddress()

            urlService.UseSpaUrlAddress();

            var confirmationUrl = urlService.BuildConfirmationUrl("123", "token123");
            
            Assert.Contains("123", confirmationUrl.ToString());
            Assert.Contains("token123", confirmationUrl.ToString());
        }
    }
}
