using Application.Customers;
using Application.Services;
using Application.Services.Business;
using Domain;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer.Tests.CourseRepo
{
    public class InstructorCourseRepositoryTest:TestBase
    {
        private readonly IRepository<Course> courseRepository;
        private IRepository<Instructor> instructoRepository;
        private IRepository<InstructorCourse> instructorCourseRepository;
        private readonly ICustomerManager customerManager;

        public InstructorCourseRepositoryTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) 
        {
            courseRepository = factory.Services.GetRequiredService<IRepository<Course>>();
            instructoRepository = factory.Services.GetRequiredService<IRepository<Instructor>>();
            instructorCourseRepository = factory.Services.GetRequiredService<IRepository<InstructorCourse>>();
            customerManager = factory.Services.GetRequiredService<ICustomerManager>();
        }


        [Fact(DisplayName = "InstructorCourse Repository adds new instructorCourse.")]
        public async Task InstructoCourseRepositoryAddNewInstructoCourse()
        {

            #region Arrange an Instructor

            string customerEmail = $"{Guid.NewGuid().ToString()}@example.com";
            Customer customer = await customerManager.FindByEmailAsync(customerEmail);

            if (customer == null)
                customer = await customerManager.CreateAsync
                          (new CustomerRegistrationRequest("John", "Fogerty", customerEmail, "RunningInTheJungle12@"));

            Assert.True(customer != null, "Customer should be created");

            var newInstructor = new Instructor();
            newInstructor.CustomerId = customer.Id;
            await instructoRepository.AddOrUpdateAsync(newInstructor);

            var instructor = await instructoRepository.FindAsync(newInstructor.Id);
            Assert.True(instructor != null, "Instructor should be created");

            #endregion

            #region Arrange course

            var newCourse = new Course();
            newCourse.Name = "Molecular Biology";
            newCourse.Descripton = "Molecular Biology Desription";

            await courseRepository.AddOrUpdateAsync(newCourse);

            var course= await courseRepository.FindAsync(newCourse.Id);

            Assert.True(course != null, "Course should be persisted in DB");
            Assert.True(course.Name == newCourse.Name, "Course.Name should be persisted in DB");

            #endregion

            #region Create instructorCourse

            var newInstructorCourse = new InstructorCourse();
            newInstructorCourse.InstructorId = instructor.Id;
            newInstructorCourse.CourseId = course.Id;

            await instructorCourseRepository.AddOrUpdateAsync(newInstructorCourse);

            var instructoCourse = await instructorCourseRepository
                                        .FindAsync(c => c.CourseId == newInstructorCourse.CourseId
                                                     && c.InstructorId == instructor.Id);


            Assert.True(instructoCourse != null, "Instructor course should be created");


            #endregion

        }
    }
}
