using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for Registering and Logging in Users
    /// </summary>
    public interface IAuthService<Tuser> where Tuser:class
    {
        Task<(Result, Tuser newUser)> RegisterAsync(UserRegisterModel model, bool sendConfirmationEmail);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> RefreshTokenAsync(string jwtToken, string refreshToken);
        Task<Result> ConfirmAccountAsync(string userId, string confirmToken);
    }
}