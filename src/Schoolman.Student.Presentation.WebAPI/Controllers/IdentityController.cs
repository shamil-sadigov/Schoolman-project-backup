using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure;

namespace Schoolman.Student.WenApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAuthService authAservice;
        public IdentityController(IAuthService authAservice)
        {
            this.authAservice = authAservice;
        }



        [HttpGet]
        public IActionResult GetValues()
        {
            return Ok("I'm alive");
        }



        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO dto)
        {
            var result = await authAservice.RegisterAsync(dto.Email, dto.Password);
            return null;
        }

    }


    public class RegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }



}
