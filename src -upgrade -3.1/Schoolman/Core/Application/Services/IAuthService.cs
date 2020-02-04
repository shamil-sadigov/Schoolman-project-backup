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
        Task<Result<User>> RegisterUserAsync(UserRegisterModel model);
        Task<Result<AuthenticationCredential>> LoginUserAsync(string email, string password);

    }
}