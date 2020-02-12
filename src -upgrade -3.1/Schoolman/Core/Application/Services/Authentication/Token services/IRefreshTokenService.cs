using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;

namespace Application.Services.Token
{
    /// <summary>
    /// Service for generating and validation refresh tokens.
    /// </summary>

    public interface IRefreshTokenService : 
        ITokenValidator<string, Result>,
        ITokenFactory<Customer, Result<string>>
    {

    }
}
