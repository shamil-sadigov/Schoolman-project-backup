using Application.Services;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Users.UserRegistration
{
    public class UserRegistrationRequestValidator:AbstractValidator<UserRegistrationRequest>
    {
        private readonly IUserService userService;

        public UserRegistrationRequestValidator(IUserService userService)
        {
            this.userService = userService;
        }


        public UserRegistrationRequestValidator()
        {
            RuleFor(model => model.FirstName).NotEmpty()
                                             .MaximumLength(25);

            RuleFor(model => model.LastName).NotEmpty()
                                            .MaximumLength(25);

            RuleFor(model => model.Password).NotEmpty()
                                            .MinimumLength(8);

            RuleFor(model => model.Email).NotEmpty();

            RuleSet("EmailDoesntExistInDb", () =>
            {
                RuleFor(model => model.Email)
                .MustAsync(async (email, token ) =>
                {
                    bool userDoesntExist = !await userService.ExistAsync(user => user.Email == email);
                    return userDoesntExist;
                });
            });

            CascadeMode = CascadeMode.Continue;
        }
    }
}
