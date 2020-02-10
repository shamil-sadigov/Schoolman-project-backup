using Application.Users;
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
    public class UserServiceTest : TestBase
    {
        private readonly IUserService userService;
        public UserServiceTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
            userService = factory.Services.GetRequiredService<IUserService>();
        }


        [Fact(DisplayName = "IUserService is registered in IoC and not null")]
        public void UserServiceIsRegisteredAndNotNull()
        {
            Assert.True(userService != null, "IUserService is not registered");
        }


        [Fact(DisplayName = "IUserService.CreateUser() => with empty FirstName and Lastname =>  fails")]
        public async Task UserService_Doesnt_Create_User_With_Emtpty_FirstName_And_Lastname()
        {
            var user = new ClientRegistraionRequest()
            {
                FirstName = "",
                LastName = ""
            };

            Result<User> creationResult = await userService.CreateAsync(user, "FortunateSon1414##");

            Assert.False(creationResult.Succeeded, "User should not be created since FirstName & Lastname is invalid");
        }


        [Fact(DisplayName = "IUserService.CreateUser() => with invalid Password => fail")]
        public async Task UserService_Doesnt_Create_User_With_Invalid_Password()
        {
            var user = new ClientRegistraionRequest()
            {
                FirstName = "",
                LastName = ""
            };

            Result<User> creationResult = await userService.CreateAsync(user, "Aa123");

            Assert.False(creationResult.Succeeded, "IUserService.CreateUser should return false since User's password is not valid");
        }



        [Fact(DisplayName = "IUserService.CreateUser() => with valid FirstName, Lastname, Email, UserName and Password => succeed")]
        public async Task UserService_Create_new_User_With_Valid_Values()
        {
            bool userCreated = await userService.CreateAsync(user: new ClientRegistraionRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com"
            }, "Proud Mary 13@#");

            Assert.True(userCreated, "IUserService.CreateUser => should return true since all User's values are valid");
        }



        [Fact(DisplayName = "IUserService.DeleteUser() => Suceed if user exist and Fails if user doesn't exist")]
        public async Task UserService_deletes_newly_created_user()
        {
            User createdUser = await userService.CreateAsync(user: new ClientRegistraionRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com"

            }, "Proud Mary 13@#");

            bool isDeleted = await userService.DeleteAsync(createdUser);
            Assert.True(isDeleted, "User should be deleted");

            isDeleted = await userService.DeleteAsync(createdUser);
            Assert.False(isDeleted, "User should not be deleted since it's been deleted before");
        }



        [Fact(DisplayName = "IUserService.CheckUserAsync() => with ConfriemdEmail")]
        public async Task UserService_CheckUserAsync_on_confirmed_email()
        {
            #region Create user and check emailconfirmation

            User createdUser = await userService.CreateAsync(user: new ClientRegistraionRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com"
            }, password: "Proud Mary 13@#");

            Assert.True(createdUser != null, "Newly created user should not be null");

            bool userEmailConfirmed = await userService.ExistAsync(u => u.Id == createdUser.Id && u.EmailConfirmed);
            Assert.False(userEmailConfirmed, "User email cannot be confirmed since we didn't sent confirmation email");


            #endregion

        }



        [Fact(DisplayName = "IUserService.CheckUserAsync() => with Password")]
        public async Task UserService_CheckUserAsync_password_match()
        {

            #region Passwords should not pass

            User user = await userService.CreateAsync(user: new ClientRegistraionRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com"
            }, password: "Creedence13!@");


            Assert.True(user != null, "Newly created user should not be null");

            bool passwordValid = await userService.CheckPasswordAsync(user, "Creedence13!@");
            Assert.True(passwordValid, "User password should be valid");

            #endregion

            #region Password should match

            User user2 = await userService.CreateAsync(user: new ClientRegistraionRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney2.com"
            }, password: "Creedence13!@");

            Assert.True(user2 != null, "Newly created user should not be null");

            passwordValid = await userService.CheckPasswordAsync(user2, "SomeInvalidPassword");
            Assert.False(passwordValid, "User password should not be valid");

            #endregion
        }

    }
}
