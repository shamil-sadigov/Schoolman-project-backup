using Application.Common.Models;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service that manages authentication tokens
    /// </summary>
    public interface IAuthTokenService
    {   
        /// <summary>
        /// Generatees access token for specified customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> GenerateAuthenticationTokensAsync(Customer customer);

        /// <summary>
        /// Take expired tokens and exchange them
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> ExchangeAuthenticationTokensAsync(AuthenticationTokens authenticationCredential);
    }
}