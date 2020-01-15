using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Infrastructure;
using Schoolman.Student.WenApi.Controllers.Identity.DTO;
using Schoolman.Student.WenApi.Controllers.Identity.DTO.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers
{
    /// <summary>
    /// Testing
    /// </summary>
    [Route("api/[controller]/[action]")]
    
    public partial class AccountController : ControllerBase
    {
        private readonly IAuthService<AppUser> authenticationService;        
        public AccountController(IAuthService<AppUser> authAservice)
        {
            this.authenticationService = authAservice;
        }


        /// <remarks>
        /// This action registers new user and sends confirmation email
        /// <br/><br/> 
        /// <b style="color: #f14d2f;align-contentalign-content:;" > 
        /// Password Must have:</b> <br/> <br/> 
        /// Digit (0123456789) <br/>
        /// Lowecase character (a-z) <br/>
        /// Uppercase character (A-Z) <br/>
        /// Non-Alphanumeric character (!@#$ ....) <br/>
        /// Length >=8 <br/>
        /// </remarks>
        /// <response code="200">Success request => New user registered and returned</response>
        /// <response code="400">Bad request. Cases: Email is not valid, Firstname or Lastname is empty, Password doesn't meet specified requirements and so on...</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegisteredModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestModel))]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Root.Children.SelectMany(s => s.Errors.Select(x => x.ErrorMessage)).ToArray();
                return BadRequest(new BadRequestModel(errors));
            }

            var (result, newUser) = await authenticationService
                                          .RegisterUserAsync(model, sendConfirmationEmail: true);

            if (result.Succeeded)
                return Ok((UserRegisteredModel) newUser);
            else
                return BadRequest(new BadRequestModel(result.Errors));
        }


        /// <remarks>
        /// This action returns access tokens (JWT + Refresh Token)
        ///  <br/> <br/> 
        /// <b style="color: #f14d2f;align-contentalign-content: ;" > 
        /// Attention Attention</b> <br/> <br/> 
        /// User must be registered before Login <br/>
        /// </remarks>
        /// <response code="200">Success request => Access tokens created and returned</response>
        /// <response code="400">Bad request => Cases: User is not registered, Email is not valid, Password is not valid and so on...</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginSuccess))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestModel))]
        public async Task<IActionResult> Login([FromBody] LoginModel dto)
        {
            var result = await authenticationService.LoginUserAsync(dto.Email, dto.Password);

            if (result.Succeeded)
                return Ok(new LoginSuccess(result.JwtToken, result.RefreshToken));

            return BadRequest( new BadRequestModel(result.Errors));
        }


        /// <remarks>
        /// This action take outdated tokens and returns new ones
        /// </remarks>
        /// <response code="200">Success request => New access tokens are returned</response>
        /// <response code="400">Bad request => Cases:Refresh token expired, JWT is not valid and so on...</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginSuccess))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(LoginFail))]
        public async Task<IActionResult> RefreshTokens([FromBody] TokenModel dto)
        {
            var result = await authenticationService.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);

            if (result.Succeeded)
                return Ok(new LoginSuccess(result.JwtToken, result.RefreshToken));

            return BadRequest(new BadRequestModel(result.Errors));
        }


        /// <remarks>
        /// This action confirms new user's email
        /// </remarks>
        /// <response code="200">Success request => Email confirmed</response>
        /// <response code="400">Bad request => Cases: userId or token are not valid </response>
        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await authenticationService.ConfirmEmailAsync(userId, token);

            if (result.Succeeded)
                return Ok("Congratulation! You dind't lie, your email address confirmed!");

            return BadRequest(new BadRequestModel(result.Errors));
        }
    }
}