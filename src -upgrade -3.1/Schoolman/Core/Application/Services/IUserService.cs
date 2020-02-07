using Application.Common.Helpers;
using Application.Models;
using Application.Services;
using Application.Users;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static FluentValidation.Validators.PredicateValidator;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService
    {
        Task<Result<User>> CreateAsync(UserRegistrationRequest user, string password);
        Task<Result> DeleteAsync(string email);
        [Obsolete]
        Task<Result> SendConfirmationEmailAsync(User user);

        [Obsolete]
        Task<Result> ConfirmEmailAsync(string userId, string token);
        Task<bool> ExistAsync(Expression<Func<User, bool>> Predicate);
        Task<User> FindAsync(Expression<Func<User, bool>> expression);
        Task<User> GetById(string userId);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<int> UpdateRefreshTokenAsync(User user, RefreshToken refreshToken);
    }
}
