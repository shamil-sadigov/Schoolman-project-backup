using Domain.Models;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Student:EntityBase<string>
    {
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        /// <summary>
        /// Reviews that student left on courses
        /// </summary>
        public ICollection<CourseReview> Reviews { get; set; }

        /// <summary>
        /// Courses that student acquired (paid for)
        /// </summary>
        public ICollection<StudentAcquiredCourse> CoursesAcquired { get; set; }
        public ICollection<StudentWishedCourses> CoursesWished { get; set; }
    }




}
