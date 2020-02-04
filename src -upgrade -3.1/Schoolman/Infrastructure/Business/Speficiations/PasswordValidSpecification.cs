using Application.Common.Helpers;
using Domain;
using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Speficiations
{
    public class PasswordValidSpecification : ISpecification<User>
    {
        private readonly UserManager<User> userManager;
        
        private string password;

        public PasswordValidSpecification(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        
        public void SetPassword(string password)
            => this.password = password;
        

        /// <summary>
        /// Before calling this method ensure you set the password via SetPassword method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result> IsSatisfied(User user)
        {
            if (string.IsNullOrEmpty(password))
                return Result.Failure("Password is not valid");

            return await userManager.CheckPasswordAsync(user, password) ?
                    Result.Success() : Result.Failure("Password doesn't match");
        }
    }
}
