using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Educational course
    /// </summary>
    public class Course:EntityBase<int>
    {
        public string Name { get; set; }
        public string Descripton { get; set; }
        public TimeSpan Duration { get; set; }
        public CourseType Type { get; set; }
        // owned entity
        public CourseFee Fee { get; set; }
        public Course()
        {
            Reviews = new HashSet<CourseReview>();
            Instructors = new HashSet<InstructorPreparedCourse>();
            StudentsAcquired = new HashSet<StudentAcquiredCourse>();
            FAQs = new HashSet<FAQ>();
        }

        public ICollection<CourseReview> Reviews { get; set; }
        /// <summary>
        /// Instructor that created this course. Usually course is created by one instructor
        /// But that is not a constraint. Course can be created by multiple Instructors
        /// </summary>
        public ICollection<InstructorPreparedCourse> Instructors { get; set; }
        /// <summary>
        /// Studens that acquired this course
        /// </summary>
        public ICollection<StudentAcquiredCourse> StudentsAcquired { get; set; }
        /// <summary>
        /// Frequently asked question regarding this course
        /// </summary>
        public ICollection<FAQ> FAQs { get; set; }

        public ICollection<StudentWishedCourses> StudentsWished { get; set; }

    }



    /// <summary>
    /// VR means Virtual Reality
    /// </summary>
    public enum CourseType : byte
    {
        InClass, Online, VR
    }




}
