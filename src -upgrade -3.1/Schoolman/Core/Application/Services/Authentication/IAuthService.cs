using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Users;
using Domain;
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
        Task<Result<Client>> RegisterClientAsync(ClientRegistraionRequest request);
        Task<Result<AuthenticationTokens>> LoginClientAsync(ClientLoginRequest request);
    }
}