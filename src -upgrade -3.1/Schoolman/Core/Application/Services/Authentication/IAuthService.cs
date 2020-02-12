using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Customers;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for Registering and Logging in Customer
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registes customer if parameters are valid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<Customer>> RegisterCustomerAsync(CustomerRegistrationRequest request);

        /// <summary>
        /// Login customer and returns authentication token that consist of access and refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<AuthenticationTokens>> LoginCustomerAsync(CustomerLoginRequest request);
    }
}