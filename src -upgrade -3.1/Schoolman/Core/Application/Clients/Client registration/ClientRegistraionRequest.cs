using Domain;
using MediatR;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users
{
    public partial class ClientRegistraionRequest : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
