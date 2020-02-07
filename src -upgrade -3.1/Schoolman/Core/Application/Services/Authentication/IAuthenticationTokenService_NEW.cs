using Application.Common.Models;
using Domain;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IAuthenticationTokenServiceRefined
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(User user);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> ExchangeAuthenticationTokensAsync(AuthenticationTokens authenticationCredential);
    }
}