using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    public class UserService : IUserService<AppUser>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfirmationEmailService emailService;
        private readonly UserSearchOptions options = new UserSearchOptions();

        public UserService(UserManager<AppUser> userManager,
                 IConfirmationEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }


        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Creation result</returns>
        public async Task<(Result result, AppUser user)> CreateUser(string email, string password)
        {
            (bool IsUserValid, string error) = await ValidateUser(email, password);

            if (!IsUserValid)
                return (Result.Failure(error), user: null);

            return await TryCreateUserAsync(email, password);
        }


        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public async Task<Result> DeleteUser(string userId)
        {
            var userToDelete = await userManager.FindByIdAsync(userId);
            
            if (userToDelete == null)
                return Result.Failure("User wasn't found");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            if (deletionResult.Succeeded)
                return Result.Success();

            return Result.Failure(deletionResult.Errors.Select(s => s.Description).ToArray());
        }



        /// <summary>
        /// Finds user based on options
        /// </summary>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public async Task<(Result, AppUser)> Find(Action<UserSearchOptions> configureOptions)
        {
            configureOptions(options);
            
            if(string.IsNullOrEmpty(options.Email))
                return (Result.Failure("Email is not valid"), null);

            // Check user by Email
            var user = await userManager.FindByEmailAsync(options.Email);
            if(user == null)
                return (Result.Failure("User doesn't exists"), null);

            // Check if Password valid
            if (!string.IsNullOrEmpty(options.PasswordToConfirm))
                if (!await userManager.CheckPasswordAsync(user, options.PasswordToConfirm))
                    return (Result.Failure("Password is not valid"), null);

            // Check if email confirmed
            if (options.ConfirmedEmail)
                if (!await userManager.IsEmailConfirmedAsync(user))
                    return (Result.Failure("Email is not confirmed"), null);

            return (Result.Success(), user);
        }


        public async Task<Result> SendConfirmationEmail(AppUser user)
        {
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await emailService.SetConfirmationOptions(token, user.UserName)
                                           .SendAsync(user.Email);
               
            return result;
        }



        public async Task<Result> ConfirmEmail(AppUser user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return Result.Failure(result.Errors.Select(e=> e.Description).ToArray());

            return Result.Success();
        }



        #region Local helper methods

        /// <summary>
        /// Verifies if email and password is valid, and whether user does exists.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Validation result</returns>
        private async Task<(bool IsUserValid, string error)> ValidateUser(string email, string password)
        {
            // Verify if email and password is valid
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return (false, "Email or password is invalid");

            // Verify if user already exists
            var user = await userManager.FindByNameAsync(email);
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
