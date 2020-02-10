using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Users;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service for Registering and Logging in Users
    /// </summary>
    public interface IAuthService
    {
        Task<Result<Customer>> RegisterCustomerAsync(CustomerRegistrationRequest request);
        Task<Result<AuthenticationTokens>> LoginCustomerAsync(CustomerLoginRequest request);
    }
}