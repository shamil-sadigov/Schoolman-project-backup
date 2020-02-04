using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IValidator<UserRegistrationRequest> validator;
        private readonly IAuthService authenticationService;

        public UserRegistrationRequestHandler(IValidator<UserRegistrationRequest> validator,
                                              IAuthService authenticationService)
        {
            this.validator = validator;
            this.authenticationService = authenticationService;
        }


        public async Task<Result> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            #region User Validation

            // Validate properties
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                string[] errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                return Result.Failure(errors);
            }

            // Validate email doesnt exists in DB
            var emailValidationResult = await validator.ValidateAsync(request, ruleSet: "EmailDoesntExistInDb");

            if (!emailValidationResult.IsValid)
            {
                string[] errors = emailValidationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                return Result.Failure(errors);
            }

            #endregion

            return await authenticationService.RegisterUserAsync(request);
        }
    }


}
