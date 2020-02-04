using Application.Common.Models;
using Application.Models;
using Domain;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for Registering and Logging in Users
    /// </summary>
    public interface IAuthService
    {
        Task<Result<User>> RegisterUserAsync(UserRegisterModel model, bool sendConfirmationEmail);
        Task<Result<AuthenticationCredentials>> LoginUserAsync(string email, string password);
        Task<Result<AuthenticationCredentials>> RefreshTokenAsync(string jwtToken, string refreshToken);
        Task<Result> ConfirmEmailAsync(string userId, string confirmToken);
    }
}