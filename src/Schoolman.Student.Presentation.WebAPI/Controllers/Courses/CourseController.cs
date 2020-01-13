using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Schoolman.Student.WenApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    public class CourseController : Controller
    {
        [HttpGet]
        public IActionResult GetCourses()
        {
            return Ok("JWT worked!");
        } 
    }
}
