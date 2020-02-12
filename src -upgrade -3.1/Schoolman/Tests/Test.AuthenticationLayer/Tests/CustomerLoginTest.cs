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



        [Fact(DisplayName = "IMediator handle CustomerLoginRequest")]
        public async Task MediatorHandleCustomerLoginRequest()
        {
            var request = new CustomerRegistrationRequest()
            {
                Email = "zoom7oom@gmail.com",
                FirstName = "asdasdsadas",
                LastName = "qweqweqwe",
                Password = "Creedence is best Band Ever1414##"
            };

            var result = await mediator.Send(request);


            var customers = await customerManager.ListAsync();

            var customer1 = customers.FirstOrDefault();

            var list = await context.Set<Customer>().ToListAsync();

            var listTraced = await context.Set<Customer>().Include(c=> c.UserInfo)
                                                          .AsNoTracking()
                                                          .FirstOrDefaultAsync(u => u.UserInfo.Email == request.Email);

            
            var listNoTracekd = await context.Set<Customer>().Include(c => c.UserInfo)
                                                    .Include(c => c.Company)
                                                    .Include(c => c.Role)
                                                    //.AsNoTracking()
                                                    .FirstOrDefaultAsync(c => c.UserInfo.Email == request.Email);


            var customer = await customerManager.FindByEmailAsync(request.Email);

            customer.UserInfo.EmailConfirmed = true;
            await customerManager.UpdateAsync(customer);

            var customerLogin = new CustomerLoginRequest()
            {
                Email = customer.UserInfo.Email,
                Password = request.Password
            };

            var loginResult = await mediator.Send(customerLogin);
            
            Assert.True(result.Succeeded);
        }

    }
}
