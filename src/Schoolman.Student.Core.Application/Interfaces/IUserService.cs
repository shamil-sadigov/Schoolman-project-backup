using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{

    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService<T> where T: class
    {
        Task<(Result result, T user)> CreateUser(string email, string password);
        Task<Result> DeleteUser(string userId);
        Task<(Result, T)> Find(Action<UserSearchOptions> searchOptions);
        Task<Result> SendConfirmationEmail(T user);
        Task<Result> ConfirmEmail(T user, string token);
    }
}
