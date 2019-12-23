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
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMemoryCache memoryCache;

        public UserService(UserManager<AppUser> userManager,
                           IMemoryCache memoryCache)
        {
            this.userManager = userManager;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Creation result</returns>
        public async Task<(Result result, string UserId)> CreateUser(string email, string password)
        {
            (bool IsUserValid, string error) = ValidateUser(email, password);

            if (!IsUserValid)
                return (Result.Failure(error), string.Empty);


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
                return Result.Failure("User wan't found");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            if (deletionResult.Succeeded)
                return Result.Success();

            return Result.Failure(deletionResult.Errors.Select(s => s.Description).ToArray());
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
        private async Task<(Result result, string userId)> TryCreateUserAsync(string email, string password)
        {
            var newUser = new AppUser(email: email, userName: email);
            var creation_result = await userManager.CreateAsync(newUser, password);

            if (!creation_result.Succeeded)
            {
                var errors = creation_result.Errors.Select(e => e.Description).ToArray();
                return (Result.Failure(errors), string.Empty);
            }

            // set newUser to cache to use it in token generation
            memoryCache.Set<AppUser>(newUser.Email, newUser, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(5)
            });

            return (Result.Success(), newUser.Id.ToString());
        }

        #endregion
    }
}
