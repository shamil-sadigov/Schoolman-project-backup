

using Application.Common.Helpers;
using Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Schoolman.Student.Core.Application.Models
{
    public class UserAssertOptions2
    {
        private readonly UserManager<User> userManager;
        List<ISpecification<User>> specifications;


        public UserAssertOptions2(UserManager<User> userManager)
        {
            specifications = new List<ISpecification<User>>();
            this.userManager = userManager;
        }

        public string Password { get; private set; }
        public bool ConfirmEmail { get; private set; }

        public UserAssertOptions PasswordToConfirm(string password)
        {
            specifications.Add(new PasswordValidSpecification())
        }


        public UserAssertOptions ShouldConfirmEmail(bool confirmed)
        {
            ConfirmEmail = confirmed;
            return this;
        }
    }
}
