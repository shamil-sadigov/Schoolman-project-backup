using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Customers;
using Application.Services.Business;
using Domain;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.AuthenticationLayer.Login
{
#warning Additional test should be added to test customer login
    public class CustomerLoginTest:TestBase
    {
        private readonly IMediator mediator;
        private readonly ICustomerManager customerManager;
        private readonly SchoolmanContext context;
        public CustomerLoginTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
             mediator = factory.Services.GetRequiredService<IMediator>();
             customerManager = factory.Services.GetRequiredService<ICustomerManager>();
             context = factory.Services.GetRequiredService<SchoolmanContext>();
        }


        [Fact(DisplayName = "IMediator is registered in IoC and not null")]
        public void IMediatorIsRegistered()
        {
            Assert.True(mediator != null, "IMediator is not registered");
        }



        [Fact(DisplayName = "IMediator handles  CustomerLoginRequest after CustomerRegistrationRequest is completed successfully")]
        public async Task MediatorHandleCustomerLoginRequest()
        {
            var request = new CustomerRegistrationRequest()
            {
                Email = $"zoom {new Random().Next(0, 9999999)}@gmail.com",
                FirstName = "asdasdsadas",
                LastName = "qweqweqwe",
                Password = "Creedence is best Band Ever1414##"
            };
            Result result = await mediator.Send(request);

            Assert.True(result.Succeeded, "New Customer should be registered");

            var customer = await customerManager.FindByEmailAsync(request.Email);

            Assert.True(customer!=null, "Customer should not be null since we just recently registered this customer");

            customer.UserInfo.EmailConfirmed = true;
            bool updated = await customerManager.UpdateAsync(customer);

            Assert.True(updated, "Customer should be updated");

            var customerLogin = new CustomerLoginRequest()
            {
                Email = customer.UserInfo.Email,
                Password = request.Password
            };
            Result<AuthenticationTokens> loginResult = await mediator.Send(customerLogin);
            
            Assert.True(result.Succeeded, "Authentication tokens should be generated for CustomerLoginRequest");
            Assert.True(loginResult.Response.RefresthToken !=null, "Refresh token should not be null after login successfully");
            Assert.True(loginResult.Response.AccessToken !=null, "Access token should not be null after login successfully");
        }
    }
}
