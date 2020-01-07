using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            var result = await authAservice.RegisterAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Confirms User account after registering
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="confirmationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Confirm(string userId, string confirmationToken)
        {   
            var result = await authAservice.ConfirmAccount(userId, confirmationToken);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Login(UserDTO dto)
        {
            var result = await authAservice.LoginAsync(dto.Email, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new AuthDTO(result.JwtToken, result.RefreshToken));
        }
    }


    public class UserDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }



    public class AuthDTO
    {
        public AuthDTO()
        {

        }

        public AuthDTO(string jwt, string refresh)
        {
            access_token = jwt;
            refresh_token = refresh;
        }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }



}
