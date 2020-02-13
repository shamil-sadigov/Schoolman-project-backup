using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Request_Handlers.Clients.Exchange_auth_tokens
{
    public  class ExchangeTokensValidator:AbstractValidator<ExchangeTokensRequest>
    {
        public ExchangeTokensValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefresthToken).NotEmpty();
        }
    }
}
