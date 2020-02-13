using Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Service that return information about current user
    /// </summary>
    public interface ICurrentCustomerService
    {
        string CurrentCustomerId();
        ClaimsPrincipal CurrentCustomerClaims();
    }
}
