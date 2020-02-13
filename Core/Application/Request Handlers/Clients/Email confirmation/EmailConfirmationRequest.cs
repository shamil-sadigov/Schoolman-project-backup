using MediatR;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Request_Handlers.Customers.Email_confirmation
{
    public class EmailConfirmationRequest : IRequest<Result>
    {
        public string ClientId { get; set; }
        public string Token { get; set; }


        public EmailConfirmationRequest()
        {

        }

        public EmailConfirmationRequest(string clientId, string token)
        {
            ClientId = clientId;
            Token = token;
        }

    }
}
