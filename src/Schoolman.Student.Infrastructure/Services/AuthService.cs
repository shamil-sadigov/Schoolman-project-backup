using Microsoft.AspNetCore.Authentication;
using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    /// <summary>
    /// Service for user authentication
    /// </summary>
    public class AuthService : IAuthService<AppUser>
    {
        private readonly IUserService<AppUser> userService;
        private readonly IJwtFactory<AppUser> tokenFactory;

        public AuthService(IUserService<AppUser> userService,
                           IJwtFactory<AppUser> tokenFactory)
        {
            this.userService = userService;
            this.tokenFactory = tokenFactory;
        }


        /// <summary>
        /// Registers user and return registration result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(Result, AppUser newUser)> RegisterUserAsync(UserRegisterModel model, bool sendConfirmationEmail)
        {
            // User creation
            var (result, newUser) = await userService.CreateUser(model);

            if (!result.Succeeded)
                return (result, newUser: null);

            if(sendConfirmationEmail)
                result = await userService.SendConfirmationEmail(newUser);

            if (!result.Succeeded)
                return (result, newUser: null);
            else
                return (result, newUser);
        }

        
        /// <summary>
        /// Generates token for registered and email-confirmed user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthResult> LoginUserAsync(string email, string password)
        {
            if(Is.NullOrWhiteSpace(email, password))
                return AuthResult.Failure("Email or password are invalid");

            var (result, user) = await userService.Find(email, ops => ops.WithPassword(password)
                                                                         .WithConfirmedEmail(true));

            if (!result.Succeeded)
                return AuthResult.Failure(result.Errors);

            return await tokenFactory.GenerateTokensAsync(user);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<AuthResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            if (Is.NullOrWhiteSpace(accessToken, refreshToken))
                return AuthResult.Failure("Access-token or Refresh-token is invalid");

            return await tokenFactory.RefreshTokensAsync(accessToken, refreshToken);
        }


       
        public async Task<Result> ConfirmEmailAsync(string userId, string confirmToken)
        {
            if (Is.NullOrWhiteSpace(userId, confirmToken))
                return AuthResult.Failure("UserId or ConfirmaToken is invalid");

            var result = await userService.ConfirmEmail(userId, confirmToken);
            return result;
        }
    }
}
