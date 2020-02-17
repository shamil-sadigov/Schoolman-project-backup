using Application.Customers;
using Application.Services.Business;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
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


        [Fact(DisplayName = "ICustomerManager.CreateAsync() => Doesn't creates customer with empty FirstName, Lastname and invalid Password")]
        public async Task ICustomerManagerCreateAsyncWithEmtpyFirstAndLastName()
        {
            var customer = new CustomerRegistrationRequest()
            {
                FirstName = "",
                LastName = "",
                Password = "asd"
            };

            Result<Customer> creationResult = await customerManager.CreateAsync(customer);
            Assert.False(creationResult.Succeeded, "Customer should not be created since FirstName & Lastname is not invalid");
        }




        [Fact(DisplayName = "ICustomerManager.CreateAsync() => Doesnt creates  customer with invalid password")]
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



        [Fact(DisplayName = "ICustomerManager.CreateAsync() => Creates customer with valid values")]
        public async Task UserService_Create_new_User_With_Valid_Values()
        {
            var customerRelationResult = await customerManager.CreateAsync(new CustomerRegistrationRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                Password = "Stevei22##as",
            });

            Assert.True(customerRelationResult.Succeeded, "Customer must be created");

            bool customerExists = await customerManager.ExistEmailAsync(customerRelationResult.Response.UserInfo.Email);
            Assert.True(customerExists, "Customer should exist in DB");
        }


        [Fact(DisplayName = "ICustomerManager.CreateAsync() => Doesn't created customer with existent email")]
        public async Task ICustomerDoesntCreatedWithExistentEmail()
        {
            var request = new CustomerRegistrationRequest()
            {
                FirstName = "Steve",
                LastName = "Corney",
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                Password = "Stevei22##as",
            };

            var result = await customerManager.CreateAsync(request);
            Assert.True(result.Succeeded, "Customer must be created");

            result = await customerManager.CreateAsync(request);
            Assert.False(result.Succeeded, "Customer should not be created since we already have one with this email and username");
        }




    }
}
