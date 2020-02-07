using Application.Common.Helpers;
using Application.Models;
using Application.Services;
using Application.Users;
using AutoMapper;
using Business.Options;
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

    public class UserService : IUserService
    {
        private readonly UrlService urlService;
        private readonly IRepository<User> userRepository;
        private readonly IMapper mapper;
        private readonly EmailTemplate emailTemplate;
        private readonly UserManager<User> userManager;
        private readonly IConfirmationEmailService emailService;

        public UserService(UserManager<User> userManager,
                 IConfirmationEmailService emailService,
                 IOptionsMonitor<EmailTemplate> templateOps,
                 UrlService confirmationUrlBuilder,
                 IRepository<User> userRepository,
                 IMapper mapper)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.urlService = confirmationUrlBuilder;
            this.userRepository = userRepository;
            this.mapper = mapper;
            emailTemplate = templateOps.Get("Confirmation");
        }


        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <returns>Creation result</returns>
        public async Task<Result<User>> CreateAsync(UserRegistrationRequest userDto, string password)
        {
            // password and Id are empty
            // will be added by userService
            var newUser = mapper.Map<User>(userDto);

            var creationResult = await userManager.CreateAsync(newUser, userDto.Password);

            if (!creationResult.Succeeded)
            {
                var errors = creationResult.Errors.Select(e => e.Description).ToArray();
                return Result<User>.Failure(errors);
            }

            return Result<User>.Success(newUser);
        }


        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public async Task<Result> DeleteAsync(string email)
        {
            var userToDelete = await userManager.FindByEmailAsync(email);

            if (userToDelete == null)
                return Result.Failure("User doens't exist");

            var deletionResult = await userManager.DeleteAsync(userToDelete);

            return Result.Success();
        }


        public async Task<User> FindAsync(Expression<Func<User, bool>> expression)
              => await userRepository.Set.AsNoTracking().SingleOrDefaultAsync(expression);


        public async Task<Result> ExistAsync(User user, Action<IUserCheckOptions> predicate)
        {
            var userOptions = new UserCheckOption(userManager);
            predicate?.Invoke(userOptions);
            return await userOptions.IsCheckPassed(user);
        }






#if RELEASE
#error Ensure you're using relevant email confirmation url based on server URL not SPA localhost
#endif
        public async Task<Result> SendConfirmationEmailAsync(User user)
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


        public async Task<Result> ConfirmEmailAsync(string userId, string token)
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

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await userManager.CheckPasswordAsync(user, password);
        

        public async Task<bool> ExistAsync(Expression<Func<User, bool>> predicate)
            => await userRepository.Set.AsNoTracking().AnyAsync(predicate);


        public async Task<User> GetById(string userId) =>
            await userRepository.Set.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        

        #endregion
    }
}
