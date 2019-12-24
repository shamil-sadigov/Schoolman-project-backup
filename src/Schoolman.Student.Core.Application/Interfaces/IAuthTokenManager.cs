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
        Task<(string jwt, string refreshToken)> GenerateTokens(T_user user);

        /// <summary>
        /// Get tokens and refresh
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<(Result result, string jwt, string refreshToken)> RefreshTokens(string jwt, string refreshToken);
    }
}