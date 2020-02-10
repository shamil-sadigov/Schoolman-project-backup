using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Services.Business;
using Application.Services.Token.Validators.User_Token_Validator;
using Application.Users;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthTokenService tokenService;
        private readonly IConfirmationEmailService emailConfirmationManager;
        private readonly IClientManager clientManager;
        private readonly ILogger<AuthService> logger;

        public AuthService(IAuthTokenService tokenService,
                           IConfirmationEmailService emailConfirmationManager,
                           IClientManager clientManager,
                           ILogger<AuthService> logger)
        {
            this.tokenService = tokenService;
            this.emailConfirmationManager = emailConfirmationManager;
            this.clientManager = clientManager;
            this.logger = logger;
        }



        public async Task<Result<AuthenticationTokens>> LoginClientAsync(ClientLoginRequest request)
        {
            Client client = await clientManager.FindAsync(WithConfirmedEmail(request.Email));

            if (client != null)
            {
                if (!await clientManager.UserService.CheckPasswordAsync(client.User, request.Password))
                {
                    logger.LogInformation("Login failed: User provided invalid password. " +
                                          "Client.Id {Id}, Client.Email {Email} ", 
                                          client.Id, client.User.Email);

                    return Result<AuthenticationTokens>.Failure("Invalid login credentials");
                }

                return await tokenService.GenerateAuthenticationTokensAsync(client);
            }

            logger.LogInformation("Login failed: User provided nonexistent Email. " +
                                  "User.Email {Email}", request.Email);

            return Result<AuthenticationTokens>.Failure("Invalid login credentials");
        }

        public async Task<Result<Client>> RegisterClientAsync(ClientRegistraionRequest request)
        {
            #region Creating User


            Result<Client> createionResult =  await clientManager.CreateAsync(request);

            if (!createionResult.Succeeded)
            {
                logger.LogInformation("Registraion failed: Client provided invalid registration values. " +
                                      "User.Email {Email}. Validation Errors: {@Errors}", 
                                        request.Email, createionResult.Errors);

                return createionResult;
            }

            Client createdClient= createionResult.Response;

            logger.LogInformation("Registration succeeded: New client have been registered." +
                                  "ClientId {clientId}, Email {clientEmail}", 
                                    createdClient.Id, createdClient.User.Email);

            #endregion

            #region Sending Email


            string token = await emailConfirmationManager.GenerateTokenAsync(createdClient);
            bool emailSent = await emailConfirmationManager.SendConfirmationEmailAsync(createdClient, token);

            if (!emailSent)
            {
                logger.LogInformation("Sending confirmation email failed: ClientId {clientId}, Email {clientEmail}",
                                  createdClient.Id, createdClient.User.Email);

                return Result<Client>.Failure("Sending confirmation email failed");
            }

            logger.LogInformation("Confirmation email sent to new registered client: ClientId {clientId}, Email {clientEmail}",
                                   createdClient.Id, createdClient.User.Email);

            #endregion

            return createdClient;
        }


        // Soon will be wrapped in Specification pattern
        private Expression<Func<Client, bool>> WithConfirmedEmail(string email)
            => client => client.User.Email == email && client.User.EmailConfirmed;
    }
}
