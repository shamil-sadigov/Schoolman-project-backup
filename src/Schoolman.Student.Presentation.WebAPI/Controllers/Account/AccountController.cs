using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure;
using Schoolman.Student.WenApi.Controllers.Identity.DTO;
using Schoolman.Student.WenApi.Controllers.Identity.DTO.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public partial class AccountController : ControllerBase
    {
        private readonly IAuthService<AppUser> authAservice;        
        public AccountController(IAuthService<AppUser> authAservice)
        {
            this.authAservice = authAservice;
        }


        /// <summary>
        /// Registers user and sends confirmation email
        /// </summary>
        /// 
        /// <remarks>
        /// <b style="color: #f14d2f;align-contentalign-content:;" > 
        /// Password Must have:</b> <br/> <br/> 
        /// Digit (0123456789) <br/>
        /// Lowecase character (a-z) <br/>
        /// Lowecase character (A-Z) <br/>
        /// Non-Alphanumeric character (!@#$ ....) <br/>
        /// Length >=8 <br/>
        /// </remarks>
        /// 
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Register_ResponseModel_OnSuccess))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Register_ResponseModel_OnFail))]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Root.Children.SelectMany(s => s.Errors.Select(x => x.ErrorMessage)).ToArray();
                return BadRequest(new Register_ResponseModel_OnFail(errors));
            }

            var (result, newUser) = 
            await authAservice.RegisterAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok((Register_ResponseModel_OnSuccess)newUser);

            return BadRequest(new Register_ResponseModel_OnFail(result.Errors));
        }


        /// <summary>
        /// Login user and return access and refresh tokens
        /// </summary>
        /// <remarks>
        /// <b style="color: #f14d2f;align-contentalign-content: ;" > 
        /// Attention Attention</b> <br/> <br/> 
        /// User must be registered before Login <br/>
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthFailModel))]
        public async Task<IActionResult> Login([FromBody] UserDTO dto)
        {
            var result = await authAservice.LoginAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok(new AuthSuccessModel(result.JwtToken, result.RefreshToken));


            return BadRequest(result.Errors);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthFailModel))]
        public async Task<IActionResult> RefreshTokens([FromBody] TokenDTO dto)
        {
            var result = await authAservice.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);

            if (result.Succeeded)
                return Ok(new AuthSuccessModel(result.JwtToken, result.RefreshToken));

            return BadRequest(result.Errors);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
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