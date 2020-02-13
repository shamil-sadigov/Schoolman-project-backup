using Application.Services;
using Application.Customers;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service that manages User class
    /// </summary>
    public interface IUserService : IServiceBase<User, string>
    {
        Task<Result<User>> CreateAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}

