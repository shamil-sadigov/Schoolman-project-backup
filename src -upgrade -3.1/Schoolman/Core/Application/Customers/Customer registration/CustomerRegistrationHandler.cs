using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class CustomerRegistrationHandler : IRequestHandler<CustomerRegistrationRequest, Result>
    {
        private readonly IValidator<CustomerRegistrationRequest> userValidator;
        private readonly IAuthService authenticationService;
        private readonly ILogger<CustomerRegistrationHandler> logger;

        public CustomerRegistrationHandler(IValidator<CustomerRegistrationRequest> validator,
                                              IAuthService authenticationService,
                                              ILogger<CustomerRegistrationHandler> logger)
        {
            this.userValidator = validator;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }


        public async Task<Result> Handle(CustomerRegistrationRequest request, CancellationToken cancellationToken)
        {
            #region User Validation

            // Validate properties
            var validationResult = userValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                string[] errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();

                logger.LogInformation("User validation failed: User.Email {Email}, Errors {@errors}",
                    validationResult.Errors);
                return Result.Failure(errors);
            }

            // Validate email doesnt exists in DB
            var emailValidationResult = await userValidator.ValidateAsync(request, ruleSet: "EmailDoesntExistInDb");

            if (!emailValidationResult.IsValid)
            {
                string[] errors = emailValidationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                logger.LogInformation("User validation failed: Invalid email." +
                                      "User.Email {Email}, Errors {@errors}",
                                        validationResult.Errors);
                return Result.Failure(errors);
            }

            #endregion

            return await authenticationService.RegisterCustomerAsync(request);
        }
    }


}
