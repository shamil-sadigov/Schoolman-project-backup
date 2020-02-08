using Application.Common.Models;
using Application.Services.Token.Validators.User_Token_Validator;
using Application.Users;
using Application.Users.User_Login;
using AutoMapper;
using Domain;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUserService userService;
        private readonly IAuthenticationTokenServiceRefined tokenService;
        private readonly IEmailConfirmationManager emailConfirmationManager;
        private readonly IMapper mapper;

        public AuthenticationService(IUserService userService,
                                     IAuthenticationTokenServiceRefined tokenService,
                                     IEmailConfirmationManager emailConfirmationManager,
                                     IMapper mapper)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.emailConfirmationManager = emailConfirmationManager;
            this.mapper = mapper;
        }



        public async Task<Result<AuthenticationTokens>> LoginUserAsync(UserLoginRequest request)
        {
            User user = await userService.FindAsync(WithConfirmedEmail(request.Email));

            if (user != null)
            {
                if (!await userService.CheckPasswordAsync(user, request.Password))
                    return Result<AuthenticationTokens>.Failure("Invalid login credentials");

                return await tokenService.GenerateAuthenticationTokensAsync(user);
            }

            return Result<AuthenticationTokens>.Failure("Invalid login credentials");
        }

        public async Task<Result<User>> RegisterUserAsync(UserRegistrationRequest request)
        {
            
            #region Creating User

            Result<User> userCreationResult = await userService.CreateAsync(request, request.Password);

            if (!userCreationResult.Succeeded)
                return userCreationResult;

            User createdUser = userCreationResult.Response;

            #endregion

            #region Sending Email

            var emailParameters = new EmailTokenCreationParameters(createdUser);
            string token = await emailConfirmationManager.GenerateTokenAsync(emailParameters);
            bool emailSent = await emailConfirmationManager.SendConfirmationEmailAsync(createdUser, token);

            if (!emailSent)
                return Result<User>.Failure("Sending confirmation email failed");

            #endregion

            return createdUser;
        }


        // Soon will be wrapped in Specification pattern
        private Expression<Func<User, bool>> WithConfirmedEmail(string email)
            => user => user.Email == email && user.EmailConfirmed;
    }
}
