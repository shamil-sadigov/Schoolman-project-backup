using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.WenApi.Controllers.Identity.DTO;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class IdentityController : ControllerBase
    {
        private readonly IAuthService authAservice;
        public IdentityController(IAuthService authAservice)
        {
            this.authAservice = authAservice;
        }


     
        /// <remarks>Registers user and send confirmation email to user</remarks>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Bad request</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            var result = await authAservice.RegisterAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthDTO))]
        public async Task<IActionResult> Login([FromBody] UserDTO dto)
        {
            var result = await authAservice.LoginAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok(new AuthDTO(result.JwtToken, result.RefreshToken));

            return BadRequest(new AuthDTO(result.Errors));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshTokens([FromBody] TokenDTO dto)
        {
            var result = await authAservice.RefreshTokenAsync(dto.access_token, dto.refresh_token);

            if (result.Succeeded)
                return Ok(new AuthDTO(result.JwtToken, result.RefreshToken));

            return BadRequest(new AuthDTO(result.Errors));
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        /// <summary>
        /// Confirms User account after registering
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="confirmationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Confirm(string userId, string confirmationToken)
        {
            var result = await authAservice.ConfirmAccountAsync(userId, confirmationToken);

            if (result.Succeeded)
                return Ok("Congratulation! You dind't lie, your email address confirmed!");

            return BadRequest(result.Errors);
        }
    }
}