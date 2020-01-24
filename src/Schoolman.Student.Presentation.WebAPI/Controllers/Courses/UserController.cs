using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Schoolman.Student.WenApi.Controllers.Courses
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        UserDataContext context;
        public UserController(UserDataContext context)
        {
            this.context = context;
        }

        /// <remarks>
        /// <b style="color: #f14d2f;align-contentalign-content: ;" > 
        /// Attention Attention</b> <br/> <br/> 
        /// This action is temporary, and used for authorization testing. <br/> Designed for development mode <br/> <br/>
        /// <b>For authenticaticated users only</b>
        /// <br/> 
        /// So, if you have access tokens, you can get all registered emails
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAllUsersEmail()
        {
            return Ok(context.Users.Select(u=> u.Email).ToList());
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNiceStatusCode()
        {
            return StatusCode(500, new { error = "Qaqa, paxmeldeyem"});
        }

    }
}
