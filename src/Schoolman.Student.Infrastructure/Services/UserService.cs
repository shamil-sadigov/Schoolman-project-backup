using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    public class UserService : IUserService<AppUser>
    {
        private readonly UserManager<AppUser> userManager;
        private UserSearchOptions options = new UserSearchOptions();

        public UserService(UserManager<AppUser> userManager)
                =>this.userManager = userManager;
        


        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Creation result</returns>
        public async Task<(Result result, AppUser user)> CreateUserAsync(string email, string password)
        {
            (bool IsUserValid, string error) = ValidateUser(email, password);

            if (!IsUserValid)
                return (Result.Failure(error), user: null);

            return await TryCreateUserAsync(email, password);
        }


        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public async Task<Result> DeleteUserAsync(string userId)
        {
            var userToDelete = await userManager.FindByIdAsync(userId);

            if (userToDelete == null)
                return Result.Failure("User wasn't found");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            if (deletionResult.Succeeded)
                return Result.Success();

            return Result.Failure(deletionResult.Errors.Select(s => s.Description).ToArray());
        }


        public async Task<(Result, AppUser)> FindAsync(Action<UserSearchOptions> configureOptions)
        {
            configureOptions(options);
            
            if(string.IsNullOrEmpty(options.Email))
                return (Result.Failure("Email is not valid"), null);

            // Check user by Email
            var user = await userManager.FindByEmailAsync(options.Email);
            if(user == null)
                return (Result.Failure("User doesn't exists"), null);

            // Check if Password valid
            if (!string.IsNullOrEmpty(options.Password))
                if (!await userManager.CheckPasswordAsync(user, options.Password))
                    return (Result.Failure("Password is not valid"), null);

            // Check if email confirmed
            if (options.ConfirmedEmail)
                if (!await userManager.IsEmailConfirmedAsync(user))
                    return (Result.Failure("Email is not confirmed"), null);

            return (Result.Success(), user);
        }



        #region Local methods

        /// <summary>
        /// Verifies if email and password is valid, and whether user does exists.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Validation result</returns>
        private (bool IsUserValid, string error) ValidateUser(string email, string password)
        {
            // Verify if email and password is valid
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return (false, "Email or password is invalid");

            // Verify if user already exists
            var user = userManager.FindByNameAsync(email);
            if (user != null)
                return (false, "User already exists");

            return (true, string.Empty);
        }


        /// <summary>
        /// Trying to create new user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        private async Task<(Result result, AppUser newUser)> TryCreateUserAsync(string email, string password)
        {
            var newUser = new AppUser(email: email, userName: email);
            var creation_result = await userManager.CreateAsync(newUser, password);

            if (!creation_result.Succeeded)
            {
                var errors = creation_result.Errors.Select(e => e.Description).ToArray();
                return (Result.Failure(errors), newUser: null);
            }

            return (Result.Success(), newUser);
        }

       

        #endregion
    }
}
