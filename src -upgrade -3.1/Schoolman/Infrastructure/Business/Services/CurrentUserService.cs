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
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext httpContext;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }


        public string CurrentUserId()
            => httpContext.User?.FindFirst("UserId")?.Value ?? null;
        

        public ClaimsPrincipal CurrentUserClaims()
            => httpContext.User ?? null;

    }
}
