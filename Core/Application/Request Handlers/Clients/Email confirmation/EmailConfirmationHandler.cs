using Application.Common.Exceptions;
using Application.Services.Business;
using Application.Services.Token.Validators.User_Token_Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Request_Handlers.Customers.Email_confirmation
{
    public class EmailConfirmationHandler : IRequestHandler<EmailConfirmationRequest, Result>
    {
        private readonly IConfirmationEmailService confirmationService;
        private readonly ICustomerManager customerManager;
        private readonly ILogger<EmailConfirmationHandler> logger;

        public EmailConfirmationHandler(IConfirmationEmailService confirmationEmailService,
                                        ICustomerManager customerManager,
                                        ILogger<EmailConfirmationHandler> logger)
        {
            confirmationService = confirmationEmailService;
            this.customerManager = customerManager;
            this.logger = logger;
        }

        public async Task<Result> Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
        {
            #region Find customer

            var customer = await customerManager.FindByIdAsync(request.ClientId);

            if (customer == null)
            {
                logger.LogError("EmailConfirmationHandler. Customer with Id {id} wasn't found" +
                                "during email confiration", request.ClientId);

                return Result.Failure("ClientId is invalid");
            }

            #endregion

            #region Validate token

            var validationResult = await confirmationService.ValidateTokenAsync(new EmailValidationParameters(customer, request.Token));

            if (customer == null)
            {
                logger.LogError("EmailConfirmationHandler. EmailValidation failed for Client {email} with errors {@errors}",
                                customer.UserInfo.Email, validationResult.Errors);

                return validationResult;
            }

            #endregion

            return Result.Success();
        }
    }
}
