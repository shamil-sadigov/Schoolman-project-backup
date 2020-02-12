using Domain;
using Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Service that builds claims for access token
    /// </summary>
    public interface IAuthTokenClaimService
    {
        Claim[] BuildClaims(Customer customer);
        string GetCustomerFromClaims(IEnumerable<Claim> claims);
    }
}