using Application.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext httpContext;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }


        public string CurrentUserId
        {
            get => httpContext.User?.FindFirst("UserId")?.Value;
        }
    }
}
