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
    public interface IUserService : IServiceBase<User>
    {
        Task<Result<User>> CreateAsync(UserRegistrationRequest user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<int> UpdateRefreshTokenAsync(User user, RefreshToken refreshToken);
    }
}
