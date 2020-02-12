using Application.Services;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer
{
    public class DbContextTest : TestBase
    {
        private readonly SchoolmanContext context;

        public DbContextTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) 
        {
            context = factory.Services.GetRequiredService<SchoolmanContext>();

        }






        [Fact(DisplayName = "SchoolmanContext is registered in IoC")]
        public void SchoolmanContextNotNull()
        {
            Assert.NotNull(context);
        }


        [Fact(DisplayName = "Schoolman Context crates Customer")]
        public async Task MediatorHandleCustomerLoginRequest()
        {

            var user = new User();
            user.Id = "UserId1";
            user.Email = "zoom7oom@gmail.com";
            user.UserName = "asd@asd.com";

            var role = new Role();
            role.Id = "RoleId11";
            role.Name = "Chemist";

            var company = new Company();
            company.Id = "CompanyId123";

            context.Add(user);
            context.Add(role);
            context.Add(company);

            bool saved = await context.SaveChangesAsync() > 0 ? true : false;

            Assert.True(saved, "User, Role and Company should be saved");





            var customer = new Customer();

            customer.Id = "CustuomerId11";
            customer.UserId = user.Id;


            context.Add(customer);
            saved = await context.SaveChangesAsync() > 0 ? true : false;

            Assert.True(saved, "Customer should be saved");


            // Context is in memory database

            // this is not null
            var customerDb1 = await context.Set<Customer>()
                                       .Include(c => c.UserInfo)
                                       .FirstOrDefaultAsync(c => c.UserInfo.Email == user.Email);

            // but this is null, the only difference in that you include more props
            // this is null because 

            var customerDb2 = await context.Set<Customer>()
                                        .Include(c=> c.UserInfo)
                                        .Include(c=> c.Role)
                                        .FirstOrDefaultAsync(c => c.UserInfo.Email == user.Email);



            Assert.True(customerDb1 != null);
        }

    }
}
