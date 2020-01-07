using Microsoft.AspNetCore.Authentication;
using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    /// <summary>
    /// Service for user authentication
    /// </summary>
    public class AuthService : IAuthService
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
        public async Task<Result> RegisterAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return AuthResult.Failure("Email or password are invalid");

            var (result, newUser) = await userService.CreateUser(email, password);

            if (!result.Succeeded)
                return result;

            result = await userService.SendConfirmationEmail(newUser);

            return result;
        }



        /// <summary>
        /// Generates token for registered and email-confirmed user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthResult> LoginAsync(string email, string password)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
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
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return AuthResult.Failure("Access-token or Refresh-token is invalid");

            return await tokenFactory.RefreshTokensAsync(accessToken, refreshToken);
        }


        public async Task<Result> ConfirmAccountAsync(string userId, string confirmToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(confirmToken))
                return AuthResult.Failure("UserId or ConfirmaToken is invalid");

            var result = await userService.ConfirmEmail(userId, confirmToken);
            return result;
        }



    }
}
