using Domain.Models;

namespace Domain.Entities
{
    public class StudentWishedCourses:EntityBase<int>
    {
        public int CourseId{ get; set; }
        /// <summary>
        /// Course that Student add to wishlist
        /// </summary>
        public Course Course { get; set; }
        public string StudentId { get; set; }

        /// <summary>
        /// Student that added Course to his wishlist
        /// </summary>
        public Student Student { get; set; }
    }




}
