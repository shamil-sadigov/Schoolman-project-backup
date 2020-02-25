using Application.Customers;
using Application.Services;
using Application.Services.Business;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer.Tests
{
    public class InctructorRepositoryTest:TestBase
    {
        private readonly IRepository<Instructor> instructorRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly ICustomerManager customerManager;

        public InctructorRepositoryTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) 
        {
            instructorRepository = factory.Services.GetRequiredService<IRepository<Instructor>>();
            customerRepository = factory.Services.GetRequiredService<IRepository<Customer>>();
            customerManager = factory.Services.GetRequiredService<ICustomerManager>();
        }



        [Fact(DisplayName = "InstructorRepository can created a new Instructor binded to a Customer")]
        public async Task InstructorRepositoryCanCreateNewInstructorBindedToACustomer()
        {
            #region Arrange: create random customer
            string customerEmail = $"{Guid.NewGuid().ToString()}@example.com";
            Customer customer = await customerManager.FindByEmailAsync(customerEmail);

            if (customer == null)
                customer = await customerManager.CreateAsync
                          (new CustomerRegistrationRequest("John", "Fogerty", customerEmail, "RunningInTheJungle12@"));

            Assert.True(customer != null, "Customer should be created");
            #endregion

            #region Act: create instructor and bind it to newly created customer

            var instructor = new Instructor();
            instructor.CustomerId = customer.Id;

            await instructorRepository.AddOrUpdateAsync(instructor);

            #endregion

            #region Assert: customer & instructor relations are saved in DB
            Instructor foundInstructor = await instructorRepository.AsQueryable()
                                                                   .Include(x => x.Customer)
                                                                   .FirstOrDefaultAsync(c => c.Id == instructor.Id);

            Assert.True(foundInstructor != null, "Instructor should be created in Db");
            Assert.True(foundInstructor.Customer.Id == customer.Id, "Customer Id should be equals since we have save it before");
            Assert.True(foundInstructor.CustomerId != customer.Id, "Customer Id should be equals since we have save it before");
            #endregion
        }



        [Fact(DisplayName = "InstructorRepository can created two different Instructors binded to the same Customer. So, one Customer can represent two different Instructors")]
        public async Task InstructorRepositoryCanCreateTwoInstructorBindedToTheSameCustomer()
        {
            #region Arrange: create a random customer
            string customerEmail = $"{Guid.NewGuid().ToString()}@example.com";

            Result<Customer> creationResult = await customerManager.CreateAsync(new CustomerRegistrationRequest("Werner", "Heisenberg", customerEmail, "Quantum Karl @##123"));
            Assert.True(creationResult.Succeeded, "Customer should be created");

            Customer newCustomer = creationResult.Response;

            #endregion

            #region Act: creates two instructors and bind them to the same Customer
            var instructor1 = new Instructor();
            instructor1.CustomerId = newCustomer.Id;
            await instructorRepository.AddOrUpdateAsync(instructor1);

            var instructor2 = new Instructor();
            instructor2.CustomerId = newCustomer.Id;
            await instructorRepository.AddOrUpdateAsync(instructor2);
            #endregion

            #region Assert: newCustomer & instructor1 relations are saved in DB


            var foundInstructor1 = await instructorRepository.AsQueryable()
                                                              .Include(x => x.Customer)
                                                              .FirstOrDefaultAsync(c => c.Id == instructor1.Id);

            Assert.True(foundInstructor1 != null, "Instructor should be created in Db");
            Assert.True(foundInstructor1.CustomerId == newCustomer.Id, "Instructor1.CustomerId should be equals since we have save it before");
            Assert.True(foundInstructor1.Customer.Id == newCustomer.Id, "Instructor1.Customer.Id should be equals since we have save it before");


            #endregion

            #region Assert: newCustomer & instructor2 relations are saved in DB

            var foundInstructor2 = await instructorRepository.AsQueryable()
                                                             .Include(x => x.Customer)
                                                             .FirstOrDefaultAsync(c => c.Id == instructor2.Id);

            Assert.True(foundInstructor2 != null, "Instructor should be created in Db");
            Assert.True(foundInstructor2.CustomerId == newCustomer.Id, "Instructor2.CustomerId should be equals since we have save it before");
            Assert.True(foundInstructor2.Customer.Id == newCustomer.Id, "Instructor2.Customer.Id should be equals since we have save it before");


            #endregion

            #region Assert: Instructors exist in customer table


            var foundCustomer = await customerRepository.AsQueryable()
                                                       .Include(c => c.Instructors)
                                                       .FirstOrDefaultAsync(c => c.Id == newCustomer.Id);



            bool instructor1ExistInCustomerTable = foundCustomer.Instructors.Any(i => i.Id == instructor1.Id);
            Assert.True(instructor1ExistInCustomerTable, "Instructor1 should exist in customer table");

            instructor1ExistInCustomerTable = foundCustomer.Instructors.Any(i => i.Id == instructor2.Id);
            Assert.True(instructor1ExistInCustomerTable, "Instructor2 should exist in customer table");

            #endregion
        }
    }
}
