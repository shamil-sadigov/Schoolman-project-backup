using Application.Customers.Registration;
using MediatR;
using Schoolman.Student.Core.Application.Models;

namespace Application.Customers
{
    /// <summary>
    /// DTO model for customer registration
    /// </summary>
    public partial class CustomerRegistrationRequest : IRequest<Result<CustomerRegistrationResponse>>
    {
        public CustomerRegistrationRequest(string firstName = default, string lastName = default, string email = default, string password = default, string phoneNumber = default)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
