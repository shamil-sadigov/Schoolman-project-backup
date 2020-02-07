

using Application.Common.Helpers;
using Business.Speficiations;
using Domain;
using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Core.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Options
{
    public class UserCheckOption: IUserCheckOptions
    {
        private readonly UserManager<User> userManager;
        readonly List<ISpecification<User>> specifications;

        public UserCheckOption(UserManager<User> userManager)
        {
            specifications = new List<ISpecification<User>>();
            this.userManager = userManager;
        }

        public IUserCheckOptions WithPassword(string password)
        {
            var passwordSpec = new PasswordValidSpecification(userManager);
            passwordSpec.SetPassword(password);
            specifications.Add(passwordSpec);
            return this;
        }

        public IUserCheckOptions WithConfirmedEmail()
        {
            var emailConfirmedSpec = new EmailConfirmedSpecification(userManager);
            specifications.Add(emailConfirmedSpec);
            return this;
        }


        internal async Task<Result> IsCheckPassed(User user)
        {
            Result result = default;

            foreach (var spec in specifications)
            {
                result = await spec.IsSatisfied(user);

                if (!result.Succeeded)
                    break;
            }

            return result;
        }

        
    }
}
