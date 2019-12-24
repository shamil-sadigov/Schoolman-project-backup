using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for generating and refreshing authentication tokens
    /// </summary>
    public interface IAuthService
    {
        Task<Result> RegisterAsync(string email, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> RefreshTokenAsync(string jwtToken, string refreshToken);
    }
}