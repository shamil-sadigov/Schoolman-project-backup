using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Schoolman.Student.Infrastructure.Services
{
    public class UserService : IUserService<AppUser>
    {
        private readonly IEmailService<ConfirmationEmailBuilder> emailService;
        private readonly UrlService urlService;
        readonly EmailTemplate emailTemplate;
        private readonly HttpContext httpContext;
        private readonly UserManager<AppUser> userManager;
        


        public UserService(UserManager<AppUser> userManager,
                 IEmailService<ConfirmationEmailBuilder> emailService,
                 IOptionsMonitor<EmailTemplate> templateOps, 
                 IHttpContextAccessor httpContextAccessor,
                 Helpers.UrlService confirmationUrlBuilder)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.urlService = confirmationUrlBuilder;
            emailTemplate = templateOps.Get("Confirmation");
            httpContext =  httpContextAccessor.HttpContext;
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
        public async Task<Result> DeleteUser(string email)
        {
            var userToDelete = await userManager.FindByEmailAsync(email);
            
            if (userToDelete == null)
                return Result.Failure("User doens't exist found");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            if (deletionResult.Succeeded)
                return Result.Success();

            return Result.Failure(deletionResult.Errors.Select(s => s.Description).ToArray());
        }



        ///// <summary>
        ///// Finds user based on options
        ///// </summary>
        ///// <param name="configureOptions"></param>
        ///// <returns></returns>
        //public async Task<(Result, AppUser)> Find(Action<UserSearchOptions> configureOptions)
        //{
        //    configureOptions(options);
            
        //    if(string.IsNullOrEmpty(options.Email))
        //        return (Result.Failure("Email is not valid"), null);

        //    // Check user by Email
        //    var user = await userManager.FindByEmailAsync(options.Email);
        //    if(user == null)
        //        return (Result.Failure("User doesn't exists"), null);

        //    // Check if Password valid
        //    if (!string.IsNullOrEmpty(options.PasswordToConfirm))
        //        if (!await userManager.CheckPasswordAsync(user, options.PasswordToConfirm))
        //            return (Result.Failure("Password is not valid"), null);

        //    // Check if email confirmed
        //    if (options.ConfirmedEmail)
        //        if (!await userManager.IsEmailConfirmedAsync(user))
        //            return (Result.Failure("Email is not confirmed"), null);

        //    return (Result.Success(), user);
        //}
        public async Task<(Result, AppUser)> Find(string email, Action<UserSearchOptions> searchOptions = null)
        {
            var user = await userManager.FindByEmailAsync(email);

            if(user == null)
                return (Result.Failure("User doesn't exists"), null);

            if (searchOptions != null)
            {
                var userOptions = new UserSearchOptions();
                searchOptions(userOptions);

                // Is password valid ?
                if(!await userManager.CheckPasswordAsync(user, userOptions.Password))
                    return (Result.Failure("Password is not valid"), null);

                // Is email confirmed ?
                if(userOptions.ConfirmedEmail)
                if(!await userManager.IsEmailConfirmedAsync(user))
                    return (Result.Failure("Email is not confirmed"), null);
            }

            return (Result.Success(), user);
        }


        public async Task<Result> SendConfirmationEmail(AppUser user)
        {
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            #region Url Encoding

            // generated token may contain some invalid characters such as '+' and '='
            // which is considered url-unsafe
            // so you should encode it as below
            token = HttpUtility.UrlEncode(token);
            // so, now '+' replaced by '%2b' 
            // and '=' by '%3d'

            #endregion

#if RELEASE
#error Ensure you're using relevant email confirmation url based on server URL not SPA localhost
#endif

            string confirmUrl = urlService.UseSpaHost()
                                          .BuildConfirmationUrl(user.Id, token);

            var result = await emailService.SendAsync(ops => ops.ConfirmationUrl(confirmUrl)
                                                                 .To(user.Email)
                                                                 .Subject("Account Confirmation")
                                                                 .Template(emailTemplate.Path));

            return result;
        }



        public async Task<Result> ConfirmEmail(string userId, string token)
        {
            var user = await  userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User doesn't exists");

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
