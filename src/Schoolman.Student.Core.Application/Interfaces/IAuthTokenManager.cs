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
        string GenerateJWT(T_user user);

        /// <summary>
        /// Generates refresh token for specified jwt Token and save it in database
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        Task<string> GenerateRefreshTokenAsync(string jwt);

        /// <summary>
        /// Check whether jwt and refresh tokens are valid
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        Task<Result> CheckRefreshToken(string refreshToken, string jwt);

        /// <summary>
        /// Get tokens and refresh
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<(string jwt, string refreshToken)> RefreshTokens(string jwt, string refreshToken);
    }
}