using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        // I decided not to inject IMediator
        // instead get it from IoC itself 
        
        public BaseController()
              => mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
        
    }
}
