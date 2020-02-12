using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Services.Business;
using Application.Services.Token.Validators.User_Token_Validator;
using Application.Customers;
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
        private readonly ICustomerManager customerManager;
        private readonly ILogger<AuthService> logger;

        public AuthService(IAuthTokenService tokenService,
                           IConfirmationEmailService emailConfirmationManager,
                           ICustomerManager customerManager,
                           ILogger<AuthService> logger)
        {
            this.tokenService = tokenService;
            this.emailConfirmationManager = emailConfirmationManager;
            this.customerManager = customerManager;
            this.logger = logger;
        }



        public async Task<Result<AuthenticationTokens>> LoginCustomerAsync(CustomerLoginRequest request)
        {
            Customer customer = await customerManager.FindByEmailAsync(request.Email);

            if (customer != null)
            {
                if (!await customerManager.CheckPasswordAsync(customer, request.Password))
                {
                    logger.LogInformation("IAuthService. Login failed: User provided invalid password. " +
                                          "Client.Id {Id}, Client.Email {Email} ", 
                                          customer.Id, customer.UserInfo.Email);

                    return Result<AuthenticationTokens>.Failure("Invalid login credentials");
                }

                return await tokenService.GenerateAuthenticationTokensAsync(customer);
            }

            logger.LogInformation("IAuthService. Login failed: User provided nonexistent Email. " +
                                  "User.Email {Email}", request.Email);

            return Result<AuthenticationTokens>.Failure("Invalid login credentials");
        }



        public async Task<Result<Customer>> RegisterCustomerAsync(CustomerRegistrationRequest request)
        {
            #region Creating User


            Result<Customer> createionResult =  await customerManager.CreateAsync(request);

            if (!createionResult.Succeeded)
            {
                logger.LogInformation("IAuthService. Registraion failed: Customer provided invalid registration values. " +
                                      "Customer.Email {customerEmail}. Validation Errors: {@Errors}", 
                                        request.Email, createionResult.Errors);

                return createionResult;
            }

            Customer newCustomer = createionResult.Response;

            logger.LogInformation("IAuthService. Registration succeeded: New customer have been registered." +
                                  "Customer.Id {customerId}, Email {customerEmail}", 
                                    newCustomer.Id, newCustomer.UserInfo.Email);

            #endregion

            #region Sending Email


            string token = await emailConfirmationManager.GenerateTokenAsync(newCustomer);
            bool emailSent = await emailConfirmationManager.SendConfirmationEmailAsync(newCustomer, token);

            if (!emailSent)
            {
                logger.LogInformation("IAuthService. Sending confirmation email failed: CustomerId {customerId}, Email {customerEmail}",
                                  newCustomer.Id, newCustomer.UserInfo.Email);

                return Result<Customer>.Failure("Sending confirmation email failed");
            }

            logger.LogInformation("IAuthService. Confirmation email sent to new registered customer: " +
                                    "CustomerId {customerId}, Email {customerEmail}",
                                   newCustomer.Id, newCustomer.UserInfo.Email);

            #endregion

            return newCustomer;
        }


        // Soon will be wrapped in Specification pattern
        private Expression<Func<Customer, bool>> WithConfirmedEmail(string email)
            => customer => customer.UserInfo.Email == email && customer.UserInfo.EmailConfirmed;
    }
}
