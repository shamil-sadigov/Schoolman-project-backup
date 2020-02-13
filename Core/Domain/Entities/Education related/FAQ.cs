using Domain.Models;

namespace Domain.Entities
{
    /// <summary>
    /// Frequently asked question of course
    /// </summary>
    public class FAQ:EntityBase<int>
    {
        public int Question { get; set; }
        public string Answer { get; set; }
        public int CourseId { get; set; }

        /// <summary>
        /// Course that is FAQ is belong to
        /// </summary>
        public Course Course { get; set; }
    }




}
