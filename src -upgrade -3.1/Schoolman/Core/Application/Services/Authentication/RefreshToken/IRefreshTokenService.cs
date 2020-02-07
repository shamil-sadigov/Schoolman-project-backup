using Domain;
using Schoolman.Student.Core.Application.Models;

namespace Application.Services.Token
{
    public interface IRefreshTokenService : 
        ITokenValidator<RefreshTokenValidationParameters, Result>,
        ITokenFactory<RefreshTokenCreationParameters, Result<string>>
    {

    }
}
