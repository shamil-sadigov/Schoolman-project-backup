using Application.Services;
using Application.Services.Business;
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
    public class ClientRegistrationRequestValidator:AbstractValidator<ClientRegistraionRequest>
    {
        private readonly IClientManager clientManager;
        public ClientRegistrationRequestValidator(IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }


        public ClientRegistrationRequestValidator()
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
                    bool userDoesntExist = !await clientManager.UserService.ExistAsync(user => user.Email == email);
                    return userDoesntExist;
                });
            });

            CascadeMode = CascadeMode.Continue;
        }
    }
}
