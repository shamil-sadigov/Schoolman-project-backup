using Domain;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IAuthTokenClaimService
    {
        Claim[] BuildClaims(User user);
        string GetUserIdFromClaims(IEnumerable<Claim> claims);
    }
}