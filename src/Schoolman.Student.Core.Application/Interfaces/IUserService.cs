using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public class UserSearchOptions
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ConfirmedEmail { get; set; }
    }

    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService<T> where T: class
    {
        Task<(Result result, T user)> CreateUserAsync(string email, string password);
        Task<Result> DeleteUserAsync(string userId);
        Task<(Result, T)> FindAsync(Action<UserSearchOptions> searchOptions);
    }
}
