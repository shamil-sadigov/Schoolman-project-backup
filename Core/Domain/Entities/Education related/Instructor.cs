using Domain.Models;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Instructor:EntityBase<string>
    {
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }

        /// <summary>
        /// Courses that Instructor prepared
        /// </summary>
        public ICollection<InstructorPreparedCourse> Courses { get; set; }
    }

}
