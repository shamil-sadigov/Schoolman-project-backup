using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;

namespace Application.Services.Token
{
    public interface IAccessTokenService:
        ITokenValidator<string, Result<ClaimsPrincipal>>,
        ITokenFactory<Customer, Result<string>>
    {
        string GetCustomerIdFromClaims(ClaimsPrincipal tokenClaims);
    }
}
