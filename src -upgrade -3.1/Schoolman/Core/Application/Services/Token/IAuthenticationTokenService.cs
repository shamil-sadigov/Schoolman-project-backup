using Application.Common.Models;
using Domain;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Manager class that responsible for generating and refreshing authentication tokens
    /// </summary>
    /// <typeparam name="T_user"></typeparam>
    public interface IAuthenticationTokenService
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<AuthenticationCredential>> GenerateAuthenticationTokensAsync(string userId);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationCredential>> ExchangeAuthenticationTokensAsync(string acessToken, string refreshToken);
    }



    public interface IAuthenticationTokenService2
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<AuthenticationCredential>> GenerateAuthenticationTokensAsync(User user);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationCredential>> ExchangeAuthenticationTokensAsync(AuthenticationCredential authenticationCredential);
    }
}