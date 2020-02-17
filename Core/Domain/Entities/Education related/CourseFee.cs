using Domain.Models;

namespace Domain.Entities
{
    // Owned entity by Course
    public class CourseFee
    {
        public int Id { get; set; }
        public decimal Amount{ get; set; }
        public CurrencyType Currency { get; set; } = CurrencyType.USD;

        public int CourseId { get; set; }
        public Course Course { get; set; }

    }

    public enum CurrencyType : byte
    {
        USD, AZN, RUB, GBP, EUR
    }
}
