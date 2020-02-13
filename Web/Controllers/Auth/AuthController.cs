using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Customers;
using Application.Customers.Registration;
using Application.Request_Handlers.Clients;
using Application.Request_Handlers.Customers.Email_confirmation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers.Auth
{

    [Route("api/auth")]

    /// <summary>
    /// Controller for authenticate clients.
    /// 1) Registration of new clients
    /// 2) Generating access tokens when clients login
    /// 3) Confirm confirmation email token
    /// 4) Exchanging expired access tokens
    /// </summary>
    public class AuthController:BaseController
    {

        /// <remarks>
        /// This action registers new client and sends confirmation email
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

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerRegistrationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestModel))]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterClient([FromBody] CustomerRegistrationRequest request)
        {
            var registrationResult =  await mediator.Send(request);

            if (registrationResult.Succeeded)
            {
                return Ok(registrationResult.Response);
            }

            return BadRequest(new BadRequestModel(registrationResult.Errors));
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationTokens))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestModel))]
        [HttpPost("login")]
        public async Task<IActionResult> LoginClient([FromBody] CustomerLoginRequest request)
        {
            var loginResult = await mediator.Send(request);

            if (loginResult.Succeeded)
            {
                return Ok(loginResult.Response);
            }

            return BadRequest(new BadRequestModel(loginResult.Errors));
        }



        /// <remarks>
        /// This action confirms new client's email
        /// </remarks>
        /// <response code="200">Success request => Email confirmed</response>
        /// <response code="400">Bad request => Cases: userId or token are not valid </response>
        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string clientId, string token)
        {
            var confirmationResult = await mediator.Send(new EmailConfirmationRequest(clientId, token));

            if (confirmationResult.Succeeded)
                return Ok();

            return BadRequest(new BadRequestModel(confirmationResult.Errors));
        }



        /// <remarks>
        /// This action take expired access token and exchange it
        /// </remarks>
        /// <response code="200">Success request: New auth tokens generated and returned in response</response>
        /// <response code="400">Bad request:  Can be if refresh token expired, JWT is not valid, or Jwt token is not expired and so on...</response>
        [HttpPost("exchangeTokens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationTokens))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestModel))]
        public async Task<IActionResult> ExchangeTokens(ExchangeTokensRequest request)
        {
            var exchangeResult = await mediator.Send(request);

            if (exchangeResult.Succeeded)
                return Ok();

            return BadRequest(new BadRequestModel(exchangeResult.Errors));
        }

    }
}
