using Application.Common.Models;
using Application.Services.Token.Validators.User_Token_Validator;
using Application.Users;
using Application.Users.User_Login;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthenticationService> logger;

        public AuthenticationService(IUserService userService,
                                     IAuthenticationTokenServiceRefined tokenService,
                                     IEmailConfirmationManager emailConfirmationManager,
                                     IMapper mapper,
                                     ILogger<AuthenticationService> logger)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.emailConfirmationManager = emailConfirmationManager;
            this.mapper = mapper;
            this.logger = logger;
        }



        public async Task<Result<AuthenticationTokens>> LoginUserAsync(UserLoginRequest request)
        {
            User user = await userService.FindAsync(WithConfirmedEmail(request.Email));
            if (user != null)
            {
                if (!await userService.CheckPasswordAsync(user, request.Password))
                {
                    logger.LogWarning("Login failed: User provided invalid password. " +
                                      "User.Id {Id}, User.Email {Email} ", 
                                       user.Id, user.Email);

                    return Result<AuthenticationTokens>.Failure("Invalid login credentials");
                }

                return await tokenService.GenerateAuthenticationTokensAsync(user);
            }

            logger.LogWarning("Login failed: User provided nonexistent Email. " +
                              "User.Email {Email}", request.Email);

            return Result<AuthenticationTokens>.Failure("Invalid login credentials");
        }

        public async Task<Result<User>> RegisterUserAsync(UserRegistrationRequest request)
        {
            
            #region Creating User

            Result<User> userCreationResult = await userService.CreateAsync(request, request.Password);

            if (!userCreationResult.Succeeded)
            {
                logger.LogWarning("Registraion failed: User provided invalid registration values. " +
                                  "User.Email {Email}. Validation Errors: {@Errors}", 
                                      request.Email, userCreationResult.Errors);

                return userCreationResult;
            }

            User createdUser = userCreationResult.Response;

            logger.LogInformation("Registration succeeded: New user have been registered." +
                                  "UserId {Id}, Email {Email}", 
                                    createdUser.Id, createdUser.Email);

            #endregion

            #region Sending Email

            var emailParameters = new EmailTokenCreationParameters(createdUser);
            string token = await emailConfirmationManager.GenerateTokenAsync(emailParameters);
            bool emailSent = await emailConfirmationManager.SendConfirmationEmailAsync(createdUser, token);

            if (!emailSent)
            {
                logger.LogWarning("Sending confirmation email failed: UserId {Id}, Email {Email}",
                                  createdUser.Id, createdUser.Email);

                return Result<User>.Failure("Sending confirmation email failed");
            }


            logger.LogInformation("Confirmation email sent to new registered user: UserId {Id}, Email {Email}",
                                   createdUser.Id, createdUser.Email);

            #endregion

            return createdUser;
        }


        // Soon will be wrapped in Specification pattern
        private Expression<Func<User, bool>> WithConfirmedEmail(string email)
            => user => user.Email == email && user.EmailConfirmed;
    }
}
