using Application.Common.Helpers;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Schoolman.Student.Core.Application.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Speficiations
{
    public class EmailConfirmedSpecification : ISpecification<User>
    {
        private readonly UserManager<User> userManager;

        public EmailConfirmedSpecification(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }


        public async Task<Result> IsSatisfied(User user)
            => await userManager.IsEmailConfirmedAsync(user) ?
                      Result.Success() : Result.Failure("Email is not confirmed");
    }


}
