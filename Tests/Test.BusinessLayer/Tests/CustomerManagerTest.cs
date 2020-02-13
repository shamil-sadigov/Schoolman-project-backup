using Application.Services.Business;
using Application.Customers;
using Domain;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.BusinessLayer
{
    public class CustomerManagerTest : TestBase
    {
        private readonly ICustomerManager customerManager;

        public CustomerManagerTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
            customerManager = factory.Services.GetRequiredService<ICustomerManager>();
        }


        [Fact(DisplayName = "ICustomerManager is registered in IoC and not null")]
        public void UserServiceIsRegisteredAndNotNull()
        {
            Assert.True(customerManager != null, "ICustomerManager is not registered");
        }


        [Fact(DisplayName = "ICustomerManager.CreateAsync() => with empty FirstName and Lastname =>  fails")]
        public async Task UserService_Doesnt_Create_User_With_Emtpty_FirstName_And_Lastname()
        {
            var customer = new CustomerRegistrationRequest()
            {
                FirstName = "",
                LastName = "",
                Password = "asdasd"
            };

            Result<Customer> creationResult = await customerManager.CreateAsync(customer);
            Assert.False(creationResult.Succeeded, "Customer should not be created since FirstName & Lastname is not invalid");
        }




        [Fact(DisplayName = "ICustomerManager.CreateUser() => with invalid Password => fail")]
        public async Task UserService_Doesnt_Create_User_With_Invalid_Password()
        {
            var user = new CustomerRegistrationRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Password = "asd"
            };

            Result<Customer> creationResult = await customerManager.CreateAsync(user);
            Assert.False(creationResult.Succeeded, "Customer should not be created since password is not invalid");
        }



        [Fact(DisplayName = "ICustomerManager.CreateUser() => with valid values => succeed")]
        public async Task UserService_Create_new_User_With_Valid_Values()
        {
            var customerRelationResult = await customerManager.CreateAsync(new CustomerRegistrationRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = "stevec@gmail.com",
                Password = "Stevei22##as",
            });

            Assert.True(customerRelationResult.Succeeded, "Customer should be created since values are valid");
        }


    }
}
