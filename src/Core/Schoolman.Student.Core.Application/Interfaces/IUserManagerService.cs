using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for CRUD Users
    /// </summary>
    public interface IUserManagerService
    {
        Task<Result> CreateUser(string username, string password);
        Task<Result> DeleteUser(string userId);
    }
}
