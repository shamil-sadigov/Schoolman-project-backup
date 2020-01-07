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
        Task<Result> DeleteUser(string email);
        Task<Result> SendConfirmationEmail(T user);
        Task<Result> ConfirmEmail(string userId, string token);
        Task<(Result, T)> Find(string email, Action<UserSearchOptions> searchOptions = null);

        //Task<(Result, T)> Find(Action<UserSearchOptions> searchOptions);
    }
}
