using Domain;
using Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IAuthTokenClaimService
    {
        Claim[] BuildClaims(Client client);
        string GetUserIdFromClaims(IEnumerable<Claim> claims);
    }
}