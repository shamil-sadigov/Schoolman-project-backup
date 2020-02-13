using Domain.Models;

namespace Domain.Entities
{
    /// <summary>
    /// One Student can acquire multiple courses
    /// One Course can be acquired by multiple Students
    /// </summary>
    public class StudentAcquiredCourse:EntityBase<int>
    {
        public AcquiredCourseStatus Status { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }

        #region Nav properties


        public Student Student { get; set; }

        /// <summary>
        /// Course that Student acquired
        /// </summary>
        public Course Course { get; set; }

        #endregion
    }


    public enum AcquiredCourseStatus : byte
    {
        Started, Completed, Quited
    }


}
