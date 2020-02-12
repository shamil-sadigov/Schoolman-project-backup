using MediatR;
using Schoolman.Student.Core.Application.Models;

namespace Application.Customers
{
    /// <summary>
    /// DTO model for customer registration
    /// </summary>
    public partial class CustomerRegistrationRequest : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

}
