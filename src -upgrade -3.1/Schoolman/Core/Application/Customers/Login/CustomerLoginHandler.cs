using Application.Clients.Client_login;
using Application.Common.Models;
using Application.Customers.Registration;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers
{
    /// <summary>
    /// Handle registraion of customer and returns result
    /// </summary>
    public class CustomerLoginHandler : IRequestHandler<CustomerLoginRequest, Result<AuthenticationTokens>>
    {
        private readonly IAuthService authenticationService;
        private readonly ILogger<CustomerRegistrationHandler> logger;

        public CustomerLoginHandler(IAuthService authenticationService,
                                    ILogger<CustomerRegistrationHandler> logger)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }


        public async Task<Result<AuthenticationTokens>> Handle(CustomerLoginRequest request, CancellationToken cancellationToken)
        {
            return await authenticationService.LoginCustomerAsync(request);
        }
    }


}
