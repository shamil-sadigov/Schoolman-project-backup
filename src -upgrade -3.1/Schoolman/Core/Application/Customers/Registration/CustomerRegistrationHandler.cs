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
    public class CustomerRegistrationHandler : IRequestHandler<CustomerRegistrationRequest, Result>
    {
        private readonly IAuthService authenticationService;
        private readonly ILogger<CustomerRegistrationHandler> logger;

        public CustomerRegistrationHandler(IAuthService authenticationService,
                                              ILogger<CustomerRegistrationHandler> logger)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }


        public async Task<Result> Handle(CustomerRegistrationRequest request, CancellationToken cancellationToken)
        {
            return await authenticationService.RegisterCustomerAsync(request);
        }
    }


}
