using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Core.Application.Common.Models
{
    public class UserRegistrationDTO
    {
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
