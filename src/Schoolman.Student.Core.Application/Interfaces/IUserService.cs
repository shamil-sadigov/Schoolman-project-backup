using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserService
    {
        Task<(Result result, string UserId)> CreateUser(string email, string password);
        Task<Result> DeleteUser(string userId);

        
    }
}
