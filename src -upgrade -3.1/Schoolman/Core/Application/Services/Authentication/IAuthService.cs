using Application.Common.Models;
using Application.Models;
using Application.Users;
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
        Task<Result<User>> RegisterUserAsync(UserRegistrationRequest model);
        Task<Result<AuthenticationTokens>> LoginUserAsync(string email, string password);
    }
}