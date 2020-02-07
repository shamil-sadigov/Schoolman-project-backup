using Application.Services.Token.Validators.Access_Token_Validator;
using Domain;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;

namespace Application.Services.Token
{
    public interface IAccessTokenService:
        ITokenValidator<AccessTokenValidationParameters, Result<ClaimsPrincipal>>,
        ITokenFactory<AccessTokenCreationParameters, Result<string>>
    {
        string GetUserIdFromClaims(ClaimsPrincipal tokenClaims);
    }
}
