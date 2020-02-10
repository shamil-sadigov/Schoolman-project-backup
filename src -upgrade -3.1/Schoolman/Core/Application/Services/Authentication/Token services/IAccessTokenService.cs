using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;

namespace Application.Services.Token
{
    public interface IAccessTokenService:
        ITokenValidator<string, Result<ClaimsPrincipal>>,
        ITokenFactory<Client, Result<string>>
    {
        string GetClientIdFromClaims(ClaimsPrincipal tokenClaims);
    }
}
