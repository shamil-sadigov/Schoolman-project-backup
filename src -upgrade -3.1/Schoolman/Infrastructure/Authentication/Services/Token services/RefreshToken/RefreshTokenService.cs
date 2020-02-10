using Application.Services.Business;
using Application.Services.Token;
using Authentication.Options;
using Domain;
using Domain.Models;
using Microsoft.Extensions.Options;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services.New_services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly RefreshTokenOptions options;
        private readonly IClientManager clientManager;

        public RefreshTokenService(IClientManager clientManager,
                                   IOptionsMonitor<RefreshTokenOptions> optionsMonitor)
        {
            this.options = optionsMonitor.CurrentValue;
            this.clientManager = clientManager;
        }


        public async Task<Result<string>> GenerateTokenAsync(Client client)
        {
            var refreshToken = new RefreshToken();
            refreshToken.IssueTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            refreshToken.ExpirationTime = DateTimeOffset.UtcNow.Add(options.ExpirationTime)
                                                                        .ToUnixTimeSeconds();

            await clientManager.AddRefreshToken(client, refreshToken);

            return Result<string>.Success(refreshToken.Token);
        }



        public async Task<Result> ValidateTokenAsync(string token)
        {
            Client client = await clientManager.FindAsync(user => user.RefreshToken.Token == token);

            if (client == null)
                return Result.Failure("Refresh token is not valid");

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (client.RefreshToken.ExpirationTime < currentTime)
                return Result.Failure("Refresh token has been expired");

            return Result.Success();
        }
    }
}
