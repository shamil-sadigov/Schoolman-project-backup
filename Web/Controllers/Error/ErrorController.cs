using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.Error
{
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }


        [Route("/error")]
        public IActionResult Error()
        {
            var exceptionContext = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if(exceptionContext.Error is EntityNotFoundException exception)
            {
                logger.LogError(exception, "ErrorController. Entity was not found with parameter {@searchParameter} was not found",
                                exception.SearchParameter);

                return NotFound(); // will be refined
            }

            logger.LogError(exceptionContext.Error, "ErrorController. Exception have been handled. See exception instance");

            return StatusCode(500, "Something went wrong ;(("); // will be refined

        }
    }
}
