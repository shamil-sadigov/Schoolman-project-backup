using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IAuthTokenManager<T_user> 
        where T_user: class
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<AuthResult> GenerateAuthTokens(T_user user);

        /// <summary>
        /// Get tokens and refresh
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<AuthResult> RefreshTokens(string jwt, string refreshToken);
    }
}