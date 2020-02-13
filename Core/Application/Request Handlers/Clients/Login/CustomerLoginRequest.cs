using Application.Common.Models;
using MediatR;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Clients.Client_login
{
    public class CustomerLoginRequest:IRequest<Result<AuthenticationTokens>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
