using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Educational course
    /// </summary>
    public class Course : EntityBase<int>
    {
        public string Name { get; set; }
        public string Descripton { get; set; }
        public TimeSpan Duration { get; set; }
        public CourseType Type { get; set; }
        public string ImageUri { get; set; }
        public Course()
        {
            Reviews = new HashSet<CourseReview>();
            Instructors = new HashSet<InstructorCourse>();
            StudentsAcquired = new HashSet<StudentAcquiredCourse>();
            FAQs = new HashSet<FAQ>();
        }

        public CourseFee Fee { get; set; }

        /// <summary>
        /// Course reviews that is left by Students
        /// </summary>
        public ICollection<CourseReview> Reviews { get; set; }
        /// <summary>
        /// Instructors that created this course. Usually course is created by one instructor
        /// But that is not a constraint. Course can be created by multiple Instructors
        /// </summary>
        public ICollection<InstructorCourse> Instructors { get; set; }
        /// <summary>
        /// Studens that acquired this course
        /// </summary>
        public ICollection<StudentAcquiredCourse> StudentsAcquired { get; set; }
        /// <summary>
        /// Studens that added this course to wishlist
        /// </summary>
        /// 
        public ICollection<StudentWishedCourses> StudentsWished { get; set; }
        /// <summary>
        /// Frequently asked question regarding this course
        /// </summary>
        public ICollection<FAQ> FAQs { get; set; }
    }


    /// <summary>
    /// VR means Virtual Reality
    /// </summary>
    public enum CourseType : byte
    {
        InClass, Online, VR
    }
}
