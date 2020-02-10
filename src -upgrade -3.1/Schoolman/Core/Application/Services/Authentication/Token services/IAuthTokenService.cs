using Application.Common.Models;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IAuthTokenService
    {   
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(Customer customer);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> ExchangeAuthenticationTokensAsync(AuthenticationTokens authenticationCredential);
    }
}