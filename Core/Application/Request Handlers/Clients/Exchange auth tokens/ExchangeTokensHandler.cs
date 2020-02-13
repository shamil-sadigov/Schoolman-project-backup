using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Request_Handlers.Clients.Exchange_auth_tokens
{
    public class ExchangeTokensHandler : IRequestHandler<ExchangeTokensRequest, Result<AuthenticationTokens>>
    {
        private readonly IAuthTokenService tokenService;
        private readonly ILogger<ExchangeTokensHandler> logger;

        public ExchangeTokensHandler(IAuthTokenService tokenService,
                                     ILogger<ExchangeTokensHandler> logger)
        {
            this.tokenService = tokenService;
            this.logger = logger;
        }

        public async Task<Result<AuthenticationTokens>> Handle(ExchangeTokensRequest request, CancellationToken cancellationToken)
        {

            var result =  await tokenService.ExchangeAuthenticationTokensAsync((AuthenticationTokens)request); // explicit conversion operator

            if (!result.Succeeded)
                logger.LogError("ExchangeTokenHandler. Exchaning auth tokens failed. Request tokens {@tokens}, errors {@errors}",
                                request, result.Errors);

            return result;
        }
    }
}
