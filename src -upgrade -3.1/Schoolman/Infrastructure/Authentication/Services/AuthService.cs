using Application.Common.Helpers;
using Application.Common.Models;
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
        private readonly IUserService userService;
        private readonly ITokenService tokenService;

        public AuthService(IUserService userService,
                           ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }


        /// <summary>
        /// Registers user and return registration result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result<User>> RegisterUserAsync(UserRegisterModel model, bool sendConfirmationEmail)
        {
            // User creation
            Result<User> userCreationResult = await userService.CreateUser(model);

            if (!userCreationResult.Succeeded)
                return userCreationResult;

            if (sendConfirmationEmail)
            {
                Result emailSendingResult = await userService.SendConfirmationEmail(userCreationResult.Response);

                if (!emailSendingResult.Succeeded)
                    return Result<User>.Failure(emailSendingResult.Errors);
            }

            return Result<User>.Success(userCreationResult.Response);
        }


        /// <summary>
        /// Generates token for registered and email-confirmed user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result<AuthenticationCredentials>> LoginUserAsync(string email, string password)
        {
            if (Assert.Is.NullOrWhiteSpace(email, password))
                return Result<AuthenticationCredentials>.Failure("User credentials invalid");

            var user = await userService.FindUserAsync(u => u.Email == email);

            if (user == null)
                return Result<AuthenticationCredentials>.Failure("User credentials invalid");

            var result = await userService.CheckUserAsync(user, ops => ops.ConfirmPassword(password)
                                                                          .ConfirmedEmail(true));
            if (!result.Succeeded)
                return Result<AuthenticationCredentials>.Failure(result.Errors);

            return await tokenService.GenerateTokensAsync(user.Id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<Result<AuthenticationCredentials>> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            if (Assert.Is.NullOrWhiteSpace(accessToken, refreshToken))
                return Result<AuthenticationCredentials>.Failure("Access-token or Refresh-token is invalid");

            return await tokenService.RefreshTokensAsync(accessToken, refreshToken);
        }



        public async Task<Result> ConfirmEmailAsync(string userId, string confirmToken)
        {
            if (Assert.Is.NullOrWhiteSpace(userId, confirmToken))
                return Result<AuthenticationCredentials>.Failure("UserId or ConfirmaToken is invalid");

            var result = await userService.ConfirmEmail(userId, confirmToken);
            return result;
        }
    }
}
