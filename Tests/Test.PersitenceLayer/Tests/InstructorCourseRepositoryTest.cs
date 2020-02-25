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
        public async Task InstructoCourseRepositoryAddsNewInstructoCourse()
        {
            #region Arrange: create new customer

            string customerEmail = $"{Guid.NewGuid().ToString()}@example.com";
            Customer customer = await customerManager.FindByEmailAsync(customerEmail);

            if (customer == null)
                customer = await customerManager.CreateAsync
                          (new CustomerRegistrationRequest("John", "Fogerty", customerEmail, "RunningInTheJungle12@"));

            Assert.True(customer != null, "Customer should be created");

            #endregion

            #region Arrange: create new instructor and bind it to created customer

            var newInstructor = new Instructor();
            newInstructor.CustomerId = customer.Id;
            await instructoRepository.AddOrUpdateAsync(newInstructor);

            var instructor = await instructoRepository.FindAsync(newInstructor.Id);
            Assert.True(instructor != null, "Instructor should be created");

            #endregion

            #region Arrage: create new course and bind it to created instructor

            var newCourse = new Course();
            newCourse.Name = "Molecular Biology";
            newCourse.Descripton = "Molecular Biology Desription";

            await courseRepository.AddOrUpdateAsync(newCourse);

            var course= await courseRepository.FindAsync(newCourse.Id);

            // Assert course created
            Assert.True(course != null, "Course should be persisted in DB");
            Assert.True(course.Name == newCourse.Name, "Course.Name should be persisted in DB");

            #endregion

            #region Act: create new instructorCourse

            var newInstructorCourse = new InstructorCourse();
            newInstructorCourse.InstructorId = instructor.Id;
            newInstructorCourse.CourseId = course.Id;

            await instructorCourseRepository.AddOrUpdateAsync(newInstructorCourse);

            #endregion

            #region Assert: instructorCourse exists in DB

            var foundIntructorCourse = await instructorCourseRepository
                                            .AsQueryable()
                                            .Include(ic => ic.Instructor)
                                            .Include(ic => ic.Course)
                                            .FirstOrDefaultAsync(ic => ic.CourseId == newInstructorCourse.CourseId
                                                                   && ic.InstructorId == instructor.Id);


            Assert.True(foundIntructorCourse != null, "InstructorCourse should be created");
            Assert.True(foundIntructorCourse.CourseId == course.Id, "InstructorCourse.CourseId should be equals to Course.Id");
            Assert.True(foundIntructorCourse.InstructorId == instructor.Id, "InstructorCourse.InstructorId should be equals to Course.Id");

            #endregion

            #region Assert: course & instructoCourse relations are saved

            bool courseRelationIsCorrect = await courseRepository.AnyAsync(c => c.Id == foundIntructorCourse.Course.Id);
            Assert.True(courseRelationIsCorrect, "Course should be in relation with InstructoCourse");

            #endregion

            #region Assert: instructor & instructorCourse relation are saved

            bool intructorRelationIsCorrect = await instructoRepository.AnyAsync(c => c.Id == foundIntructorCourse.Instructor.Id);
            Assert.True(intructorRelationIsCorrect, "Instructor should be in relation with InstructoCourse");

            #endregion
        }
    }
}
