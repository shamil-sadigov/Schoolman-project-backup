using Application.Common.Helpers;
using Application.Models;
using Domain;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{

    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService
    {
        Task<Result<User>> CreateUser(User userRegisterModel, string password);
        Task<Result> DeleteUser(string email);
        Task<Result> SendConfirmationEmail(User user);
        Task<Result> ConfirmEmail(string userId, string token);
        Task<Result> CheckUserAsync(User user, Action<IUserCheckOptions> predicate);
        Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
    }
}
