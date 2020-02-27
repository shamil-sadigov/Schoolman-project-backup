using Domain.Models;

namespace Domain.Entities
{
    /// <summary>
    /// One Instructor can create multiple courses
    /// One Course can be created buy multiple Intructors
    /// </summary>
    public class InstructorCourse:EntityBase<int>
    {
        public PreparedCourseStatus Status { get; set; }
        public int CourseId { get; set; }
        public string InstructorId { get; set; }

        public InstructorCourse()
        {

        }

        public InstructorCourse(int courseId, string instructorId)
        {
            CourseId = courseId;
            InstructorId = instructorId;
        }
        #region Nav properties
        /// <summary>
        /// Course that Instructor prepared
        /// </summary>
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }

        #endregion
    }

    public enum PreparedCourseStatus : byte
    {
        Registered, Pending, Published
    }
}
