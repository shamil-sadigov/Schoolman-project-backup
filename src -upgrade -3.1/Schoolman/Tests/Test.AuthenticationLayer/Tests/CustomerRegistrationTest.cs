using Application.Common.Models;
using Application.Customers;
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

namespace Test.AuthenticationLayer
{
    public class CustomerRegistrationTest:TestBase
    {

        private readonly IMediator mediator;

        public CustomerRegistrationTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
             mediator = factory.Services.GetRequiredService<IMediator>();
        }



        [Fact(DisplayName = "IMediator is registered in IoC and not null")]
        public void IMediatorIsRegistered()
        {
            Assert.True(mediator != null, "IMediator is not registered");
        }



        [Fact(DisplayName = "IMediator handle CustomerRegistrationRequest")]
        public async Task MediatorHandleCustomerRegistrationRequest()
        {
            var customerRequest = new CustomerRegistrationRequest()
            {
                Email = "sov.shamil@gmail.com",
                FirstName = "asdasdsadas",
                LastName = "qweqweqwe",
                Password = "Creedence is best Band Ever1414##"
            };

            var result = await mediator.Send(customerRequest);

            Assert.True(result.Succeeded);
        }

    }
}
