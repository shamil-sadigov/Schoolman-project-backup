using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;

namespace Application.Services.Token
{
    public interface IRefreshTokenService : 
        ITokenValidator<string, Result>,
        ITokenFactory<Customer, Result<string>>
    {

    }
}
