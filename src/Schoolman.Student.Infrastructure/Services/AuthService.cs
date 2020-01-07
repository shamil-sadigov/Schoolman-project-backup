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
            var (result, newUser) = await userService.CreateUser(email, password);

            if (!result.Succeeded)
                return result;

            result = await userService.SendConfirmationEmail(newUser);

            //if(!result.Succeeded)
            //    // some logging will be 

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
            var (result, user) = await userService.Find(email, ops => ops.WithPassword(password)
                                                                         .WithConfirmedEmail(true));

            if (!result.Succeeded)
                return AuthResult.Failure(result.Errors);

            return await tokenFactory.GenerateTokens(user);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public Task<AuthResult> RefreshTokenAsync(string jwtToken, string refreshToken)
        {
            return tokenFactory.RefreshTokens(jwtToken, refreshToken);
        }


        public async Task<Result> ConfirmAccount(string userId, string confirmToken)
        {
            var result = await userService.ConfirmEmail(userId, confirmToken);
            return result;
        }

    }
}
