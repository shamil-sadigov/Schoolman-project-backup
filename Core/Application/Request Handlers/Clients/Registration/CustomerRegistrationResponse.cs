namespace Application.Customers.Registration
{
    /// <summary>
    /// DTO object that we return when customer is registered
    /// </summary>
    public class CustomerRegistrationResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
