using Domain;
using Schoolman.Student.Core.Application.Models;

namespace Application.Services.Token
{
    public interface IRefreshTokenService : 
        ITokenValidator<string, Result>,
        ITokenFactory<User, Result<string>>
    {

    }
}
