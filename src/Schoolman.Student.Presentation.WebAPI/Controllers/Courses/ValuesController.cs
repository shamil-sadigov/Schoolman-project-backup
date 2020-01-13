using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(context.Users.Select(u=> u.Email).ToList());
        }
    }
}
