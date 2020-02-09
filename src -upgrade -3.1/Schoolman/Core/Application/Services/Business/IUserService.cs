using Application.Services;
using Application.Users;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService : IServiceBase<User, string>
    {
        Task<Result<User>> CreateAsync(UserRegistrationRequest user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> AddRefreshToken(User user, RefreshToken refreshToken);
        Task<IEnumerable<string>> GetRoles(User user, string roleName);
    }
}
