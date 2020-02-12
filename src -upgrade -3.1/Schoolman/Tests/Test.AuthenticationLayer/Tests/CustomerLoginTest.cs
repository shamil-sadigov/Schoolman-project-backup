using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Customers;
using Application.Services.Business;
using Domain;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.AuthenticationLayer.Login
{
    public class CustomerLoginTest:TestBase
    {

        private readonly IMediator mediator;
        private readonly ICustomerManager manager;

        public CustomerLoginTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
             mediator = factory.Services.GetRequiredService<IMediator>();
             manager = factory.Services.GetRequiredService<ICustomerManager>();
        }



        [Fact(DisplayName = "IMediator is registered in IoC and not null")]
        public void IMediatorIsRegistered()
        {
            Assert.True(mediator != null, "IMediator is not registered");
        }



        [Fact(DisplayName = "IMediator handle CustomerLoginRequest")]
        public async Task MediatorHandleCustomerLoginRequest()
        {
            var customerRequest = new CustomerRegistrationRequest()
            {
                Email = "zoom7oom@gmail.com",
                FirstName = "asdasdsadas",
                LastName = "qweqweqwe",
                Password = "Creedence is best Band Ever1414##"
            };

            var result = await mediator.Send(customerRequest);

            var customer = await manager.FindAsync(c => c.User.Email == customerRequest.Email);
            customer.User.EmailConfirmed = true;
            await manager.UpdateAsync(customer);


            var customerLogin = new CustomerLoginRequest()
            {
                Email = customer.User.Email,
                Password = customerRequest.Password
            };

            var loginResult = await mediator.Send(customerLogin);
            

            Assert.True(result.Succeeded);
        }

    }
}
