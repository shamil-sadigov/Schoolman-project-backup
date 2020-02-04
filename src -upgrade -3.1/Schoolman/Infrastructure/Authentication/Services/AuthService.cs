using Application.Common.Helpers;
using Application.Models;
using Domain;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    /// <summary>
    /// Service for user authentication
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserService userManager;
        private readonly ITokenManager tokenManager;

        public AuthService(IUserService userService,
                           ITokenManager tokenManager)
        {
            this.userManager = userService;
            this.tokenManager = tokenManager;
        }


        /// <summary>
        /// Registers user and return registration result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(Result, User newUser)> RegisterUserAsync(UserRegisterModel model, bool sendConfirmationEmail)
        {
            // User creation
            var (result, newUser) = await userManager.CreateUser(model);

            if (!result.Succeeded)
                return (result, newUser: null);

            if (sendConfirmationEmail)
                result = await userManager.SendConfirmationEmail(newUser);

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
            if (Assert.Is.NullOrWhiteSpace(email, password))
                return AuthResult.Failure("User credentials invalid");

            var user = await userManager.FindUserAsync(u => u.Email == email);

            if (user == null)
                return AuthResult.Failure("User credentials invalid");

            var result = await userManager.CheckUserAsync(user, ops => ops.ConfirmPassword(password)
                                                                          .ConfirmedEmail(true));
            if (!result.Succeeded)
                return AuthResult.Failure(result.Errors);

            return await tokenManager.GenerateTokensAsync(user.Id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<AuthResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            if (Assert.Is.NullOrWhiteSpace(accessToken, refreshToken))
                return AuthResult.Failure("Access-token or Refresh-token is invalid");

            return await tokenManager.RefreshTokensAsync(accessToken, refreshToken);
        }



        public async Task<Result> ConfirmEmailAsync(string userId, string confirmToken)
        {
            if (Assert.Is.NullOrWhiteSpace(userId, confirmToken))
                return AuthResult.Failure("UserId or ConfirmaToken is invalid");

            var result = await userManager.ConfirmEmail(userId, confirmToken);
            return result;
        }
    }
}
