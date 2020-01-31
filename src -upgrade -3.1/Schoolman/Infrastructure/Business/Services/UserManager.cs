using Application.Models;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Business.Services
{

    public class UserManager : IUserManager
    {
        private readonly IEmailService<ConfirmationEmailBuilder> emailService;
        private readonly UrlService urlService;
        private readonly IRepository<User> userRepository;
        private readonly EmailTemplate emailTemplate;
        private readonly UserManager<User> userManager;

        public UserManager(UserManager<User> userManager,
                 IEmailService<ConfirmationEmailBuilder> emailService,
                 IOptionsMonitor<EmailTemplate> templateOps,
                 UrlService confirmationUrlBuilder,
                 IRepository<User> userRepository)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.urlService = confirmationUrlBuilder;
            this.userRepository = userRepository;
            emailTemplate = templateOps.Get("Confirmation");
        }



        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <returns>Creation result</returns>
        public async Task<(Result result, User user)> CreateUser(UserRegisterModel model)
        {
            if (await UserExists(model.Email))
                return (Result.Failure("User with this email already registered"), user: null);

            return await TryCreateUserAsync(model);
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
                return Result.Failure("User doens't exist");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            if (deletionResult.Succeeded)
                return Result.Success();

            return Result.Failure(deletionResult.Errors.Select(s => s.Description).ToArray());
        }


        // return first use in collection

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> expression)
              => await userRepository.Collection.AsNoTracking().SingleOrDefaultAsync(expression);
        



        public async Task<Result> CheckUserAsync(User user, Action<UserAssertOptions> predicate)
        {
            var userOptions = new UserAssertOptions();

            predicate?.Invoke(userOptions);

            if (userOptions.Password != null)
                if (!await userManager.CheckPasswordAsync(user, userOptions.Password))
                    return Result.Failure("User credentials invalid");


            if (userOptions.ConfirmEmail)
                if (!await userManager.IsEmailConfirmedAsync(user))
                    return Result.Failure("Email is not confirmed");

            return Result.Success();
        }











#if RELEASE
#error Ensure you're using relevant email confirmation url based on server URL not SPA localhost
#endif
        public async Task<Result> SendConfirmationEmail(User user)
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

            Uri confirmUrl = urlService.UseSpaUrlAddress()
                                       .BuildConfirmationUrl
                                        (user.Id.ToString(), token);

            var result = await emailService.SendAsync(ops => ops.ConfirmationUrl(confirmUrl.ToString())
                                           .To(user.Email)
                                           .Subject("Account Confirmation")
                                           .Template(emailTemplate.Path));

            // Some logging soon....

            return result;
        }


        public async Task<Result> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User doesn't exists");

            var encodedToken = HttpUtility.UrlDecode(token);
            var result = await userManager.ConfirmEmailAsync(user, encodedToken);

            if (!result.Succeeded)
                return Result.Failure(result.Errors.Select(e => e.Description).ToArray());

            return Result.Success();
        }


        #region Local helper methods

        /// <summary>
        /// Returns bool whethere User already registered or not
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>Validation result</returns>
        private async Task<bool> UserExists(string email)
        {
            // Verify if user already exists
            var user = await userManager.FindByNameAsync(email);
            if (user != null)
                return true;

            return false;
        }


        private async Task<(Result result, User newUser)> TryCreateUserAsync(UserRegisterModel model)
        {
            var newUser = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var creation_result = await userManager.CreateAsync(newUser, model.Password);

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
