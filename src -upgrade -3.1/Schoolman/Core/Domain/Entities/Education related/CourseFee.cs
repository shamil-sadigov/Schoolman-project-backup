namespace Domain.Entities
{
    // Owned entity by Course
    public class CourseFee
    {
        public decimal Amount{ get; set; }
        public CurrencyType Currency { get; set; }
    }

    public enum CurrencyType : byte
    {
        USD, AZN, RUB, GBP, EUR
    }
}
