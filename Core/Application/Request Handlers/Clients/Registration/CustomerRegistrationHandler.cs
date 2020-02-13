using Application.Customers.Registration;
using AutoMapper;
using Domain;
using Domain.Models;
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
    public class CustomerRegistrationHandler : IRequestHandler<CustomerRegistrationRequest, Result<CustomerRegistrationResponse>>
    {
        private readonly IAuthService authenticationService;
        private readonly ILogger<CustomerRegistrationHandler> logger;
        private readonly IMapper mapper;

        public CustomerRegistrationHandler(IAuthService authenticationService,
                                           ILogger<CustomerRegistrationHandler> logger,
                                           IMapper mapper)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
            this.mapper = mapper;
        }


        public async Task<Result<CustomerRegistrationResponse>> Handle(CustomerRegistrationRequest request, CancellationToken cancellationToken)
        {
            var registrationResult = await authenticationService.RegisterCustomerAsync(request);

            if (!registrationResult.Succeeded)
            {
                logger.LogError("CustomerRegistrationHandler. Customer registration failed. Request email {email}, errors {@errors}",
                                request.Email, registrationResult.Errors);

                return Result<CustomerRegistrationResponse>.Failure(registrationResult.Errors);
            }

            var response = mapper.Map<CustomerRegistrationResponse>(registrationResult.Response);

            return Result<CustomerRegistrationResponse>.Success(response);
        }
    }
}
