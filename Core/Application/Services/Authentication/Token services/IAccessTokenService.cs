using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;

namespace Application.Services.Token
{
    /// <summary>
    /// Service for generating and validation access tokens. For example: Jwt
    /// </summary>
    public interface IAccessTokenService:
        ITokenValidator<string, Result<ClaimsPrincipal>>,
        ITokenFactory<Customer, Result<string>>
    {
        string GetCustomerIdFromClaims(ClaimsPrincipal tokenClaims);
    }
}
