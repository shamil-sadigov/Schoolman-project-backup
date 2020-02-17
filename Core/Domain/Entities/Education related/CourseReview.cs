using Domain.Models;

namespace Domain.Entities
{
    public class CourseReview:EntityBase<string>
    {
        public string Text { get; set; }
        /// <summary>
        /// Grade is a count of starts that estimates course quality
        /// </summary>
        public byte? Grade { get; set; }
        public int CourseId { get; set; }
        public string StudentId { get; set; }

        #region Nav properties

        /// <summary>
        /// Course that is review belongs to
        /// </summary>
        public Course Course { get; set; }

        /// <summary>
        /// Stundet that left a review
        /// </summary>
        public Student Student { get; set; }
        #endregion
    }




}
