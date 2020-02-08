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
    public class UserServiceTest : BasicTest
    {
        private readonly IUserService userService;
        public UserServiceTest(TestWebAppFactory testWebAppFactory):base(testWebAppFactory)
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
            var user = new User()
            {
                FirstName = "",
                LastName = ""
            };

            Result<User> creationResult = await userService.CreateUserAsync(user, "FortunateSon1414##");

            Assert.False(creationResult.Succeeded, "IUserService.CreateUser should return false since User's Firsname and Lastname are empty");
        }


        [Fact(DisplayName = "IUserService.CreateUser() => with invalid Password => fail")]
        public async Task UserService_Doesnt_Create_User_With_Invalid_Password()
        {
            var user = new User()
            {
                FirstName = "Steve",
                LastName = "Corney"
            };

            Result<User> creationResult = await userService.CreateUserAsync(user, "Aa123");

            Assert.False(creationResult.Succeeded, "IUserService.CreateUser should return false since User's password is not valid");
        }



        [Fact(DisplayName = "IUserService.CreateUser() => with valid FirstName, Lastname, Email, UserName and Password => succeed")]
        public async Task UserService_Create_new_User_With_Valid_Values()
        {
            bool userCreated = await userService.CreateUserAsync(user: new User()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com",
                UserName = "SteveCorney"
            }, "Proud Mary 13@#");

            Assert.True(userCreated, "IUserService.CreateUser => should return true since all User's values are valid");
        }



        [Fact(DisplayName = "IUserService.DeleteUser() => Suceed if user exist and Fails if user doesn't exist")]
        public async Task UserService_deletes_newly_created_user()
        {
            User createdUser = await userService.CreateUserAsync(user: new User()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com",
                UserName = "SteveCorney"

            }, "Proud Mary 13@#");

            bool isDeleted = await userService.DeletedAsync(createdUser.Email);

            Assert.True(isDeleted, "IUserService.DeleteUser => Should delete newly created user");
            
            isDeleted = await userService.DeletedAsync(createdUser.Email);

            Assert.False(isDeleted, "IUserSerivce.DeleteUser => Shouldn't delete user since user has already been deleted");
        }



        [Fact(DisplayName = "IUserService.CheckUserAsync() => with ConfriemdEmail")]
        public async Task UserService_CheckUserAsync_on_confirmed_email()
        {
            #region CheckUserAsync for User whose EmailConfirmed=false

            User user = await userService.CreateUserAsync(user: new User()
            {
                Id = NewId(),
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com",
                UserName = "SteveCorney"
            }, password: "Proud Mary 13@#");

            Assert.True(user!=null, "Newly created user should not be null");

            bool checkPassed = await userService.ExistAsync(user: user,
                                                                         predicate: ops =>
                                                                           ops.WithConfirmedEmail());

            Assert.False(checkPassed, "CheckUserAsync shoud be FALSE since user.EmailConfirmed=false");


            #endregion

            #region CheckUserAsync for User whose EmailConfirmed=true

            User user2 = await userService.CreateUserAsync(user: new User()
            {
                Id = NewId(),
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney2.com",
                UserName = "SteveCorney22",
                EmailConfirmed = true
            }, password: "Rolling on the river@#3");

            Assert.True(user2 != null, "Newly created user2 should not be null");

            checkPassed = await userService.ExistAsync(user: user2,
                                                              predicate: ops =>
                                                                  ops.WithConfirmedEmail());

            Assert.True(checkPassed, "CheckUserAsync should be TRUE since user.EmailConfirmed=true");
            #endregion
        }



        [Fact(DisplayName = "IUserService.CheckUserAsync() => with Password")]
        public async Task UserService_CheckUserAsync_password_match()
        {

            #region Passwords should not pass

            User user = await userService.CreateUserAsync(user: new User()
            {
                Id = NewId(),
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney.com",
                UserName = "SteveCorney"
            }, password: "Creedence13!@");


            Assert.True(user != null, "Newly created user should not be null");

            bool checkPassed = await userService.ExistAsync(user, ops => ops.WithPassword("Creedence13!@"));

            Assert.True(checkPassed, "CheckUserAsync should return true since password is valid");

            #endregion

            #region Password should match

            User user2 = await userService.CreateUserAsync(user: new User()
            {
                Id = NewId(),
                FirstName = "Steve",
                LastName = "Corney",
                Email = "steve@corney2.com",
                UserName = "SteveCorney2"
            }, password: "Creedence13!@");

            Assert.True(user2 != null, "Newly created user2 should not be null");

            checkPassed = await userService.ExistAsync(user, ops => ops.WithPassword("invalidPassword"));

            Assert.False(checkPassed, "CheckUserAsync should return false since password is invalid");

            #endregion

        }



        private string NewId() => Guid.NewGuid().ToString();
    }
}
