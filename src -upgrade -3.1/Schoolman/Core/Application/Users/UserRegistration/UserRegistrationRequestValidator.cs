using Application.Services;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Users.UserRegistration
{
    class UserRegistrationRequestValidator:AbstractValidator<UserRegistrationRequest>
    {
        private readonly IRepository<User> userRepository;

        public UserRegistrationRequestValidator(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
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
                    bool userExist = await userRepository.Set.AnyAsync(user => user.Email == email);
                    return !userExist;
                });
            });

            CascadeMode = CascadeMode.Continue;
        }
    }
}
