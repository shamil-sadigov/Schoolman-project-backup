using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    /// <summary>
    /// Service for user authentication
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserService<AppUser> userService;
        private readonly IAuthTokenManager<AppUser> tokenManager;

        public AuthService(IUserService<AppUser> userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Registers user and return registration result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result> RegisterAsync(string email, string password)
        {
            var (result, newUser) = await userService.CreateUserAsync(email, password);

            if (!result.Succeeded)
                return result;

            return Result.Success();
        }


        /// <summary>
        /// Generates token for registered and email-confirmed user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var (result, user) =  await userService.FindAsync(with =>
                                                               {
                                                                    with.Email = email;
                                                                    with.PasswordToConfirm = password;
                                                                    with.ConfirmedEmail = true;
                                                               });

            if (!result.Succeeded)
                return AuthResult.Failure(result.Errors);
                                                                    // Extension method
            (string jwt, string refresh_token) = await tokenManager.GenerateAuthTokens(user);

            return AuthResult.Success(jwt, refresh_token);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public Task<AuthResult> RefreshTokenAsync(string jwtToken, string refreshToken)
        {
            throw new System.Exception();
        }

        
    }
}
