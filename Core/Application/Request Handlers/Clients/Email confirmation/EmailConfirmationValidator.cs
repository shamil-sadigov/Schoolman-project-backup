using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Request_Handlers.Customers.Email_confirmation
{
    public class EmailConfirmationValidator:AbstractValidator<EmailConfirmationRequest>
    {
        public EmailConfirmationValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty()
                                    .NotNull();

            RuleFor(x => x.Token).NotEmpty()
                                  .NotNull();

        }
    }
}
