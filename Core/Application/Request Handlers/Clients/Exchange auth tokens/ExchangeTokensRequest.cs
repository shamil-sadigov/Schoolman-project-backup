using Application.Common.Models;
using MediatR;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Request_Handlers.Clients
{
    public class ExchangeTokensRequest : IRequest<Result<AuthenticationTokens>>
    {
        public string AccessToken { get; set; }
        public string RefresthToken { get; set; }

        public ExchangeTokensRequest(string accessToken, string refreshToken)
            => (AccessToken, RefresthToken) = (accessToken, refreshToken);

        public ExchangeTokensRequest()
        {

        }
    }
}
