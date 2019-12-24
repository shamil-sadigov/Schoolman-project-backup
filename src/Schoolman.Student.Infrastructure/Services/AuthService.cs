using Schoolman.Student.Core.Application;
using Schoolman.Student.Core.Application.Helpers;
using Schoolman.Student.Core.Application.Interfaces;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService<AppUser> userService;
        private readonly IAuthTokenManager<AppUser> tokenManager;

        public AuthService(IUserService<AppUser> userService)
        {
            this.userService = userService;
        }


        public async Task<Result> RegisterAsync(string email, string password)
        {
            var (result, newUser) = await userService.CreateUserAsync(email, password);

            if (!result.Succeeded)
                return result;

            return Result.Success();
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var (result, user) =  await userService.FindAsync(with =>
                                                               {
                                                                    with.Email = email;
                                                                    with.Password= password;
                                                                    with.ConfirmedEmail = true;
                                                               });

            if (!result.Succeeded)
                return AuthResult.Failure(result.Errors);
                                                                    // Extension method
            (string jwt, string refresh_token) = await tokenManager.GenerateTokensAsync(user);

            return AuthResult.Success(jwt, refresh_token);
        }

        public Task<AuthResult> RefreshTokenAsync(string jwtToken, string refreshToken)
        {
            throw new System.Exception();
        }

        
    }
}
