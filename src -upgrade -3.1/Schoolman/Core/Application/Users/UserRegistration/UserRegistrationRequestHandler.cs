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
    public class UserRegistrationRequestHandler : IRequestHandler<UserRegistrationRequest, Result>
    {
        private readonly IValidator<UserRegistrationRequest> userValidator;
        private readonly IAuthService authenticationService;
        private readonly ILogger<UserRegistrationRequestHandler> logger;

        public UserRegistrationRequestHandler(IValidator<UserRegistrationRequest> validator,
                                              IAuthService authenticationService,
                                              ILogger<UserRegistrationRequestHandler> logger)
        {
            this.userValidator = validator;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }


        public async Task<Result> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
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

            return await authenticationService.RegisterUserAsync(request);
        }
    }


}
