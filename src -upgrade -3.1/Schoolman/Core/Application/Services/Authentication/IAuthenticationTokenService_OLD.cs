using Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{

    [Obsolete]
    /// <summary>
    /// Manager class that responsible for generating and refreshing authentication tokens
    /// </summary>
    /// <typeparam name="T_user"></typeparam>
    public interface IAuthenticationTokenServiceOld
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(string userId);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> ExchangeAuthenticationTokensAsync(string acessToken, string refreshToken);
    }
}