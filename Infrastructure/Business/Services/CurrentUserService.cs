using Application.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CurrentUserService : ICurrentCustomerService
    {
        private readonly HttpContext httpContext;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }


        public string CurrentCustomerId()
            => httpContext.User?.FindFirst("UserId")?.Value ?? null;
        

        public ClaimsPrincipal CurrentCustomerClaims()
            => httpContext.User ?? null;

    }
}
