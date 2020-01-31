using Domain;
using System.Security.Claims;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface ITokenClaimsBuilder
    {
        Claim[] Build(User user);
    }
}