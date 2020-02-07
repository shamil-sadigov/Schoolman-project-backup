using Domain;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Test.Shared.Preparations;
using Xunit;

namespace BusinessLayerTest
{
    public class UserServiceTest : BasicTest
    {
        public UserServiceTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) { }

        [Fact(DisplayName = "IUserService is registered in IoC and not null")]
        public void UserServiceIsRegisteredAndNotNull()
        {
            var userService = factory.Services.GetRequiredService<IUserService>();
            Assert.NotNull(userService);

        }


        [Fact(DisplayName = "IUserService doesnt create User with empty Firsname and Lastname")]
        public async Task UserServiceIsRegisteredAndNotNull2()
        {
            var userService = factory.Services.GetRequiredService<IUserService>();

            var user = new User()
            {
                FirstName = "",
                LastName = ""
            };

            Result<User> creationResult = await userService.CreateUser(user, "Gibson141@as");

            Assert.False(creationResult.Succeeded);
        }
    }
}
