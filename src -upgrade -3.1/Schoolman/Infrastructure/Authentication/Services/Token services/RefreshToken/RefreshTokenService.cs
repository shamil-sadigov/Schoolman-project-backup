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
        private readonly IUserService userService;
        private readonly RefreshTokenOptions options;

        public RefreshTokenService(IUserService userService,
                                   IOptionsMonitor<RefreshTokenOptions> optionsMonitor)
        {
            this.userService = userService;
            this.options = optionsMonitor.CurrentValue;
        }


        public async Task<Result<string>> GenerateTokenAsync(User user)
        {
            var refreshToken = new RefreshToken();
            refreshToken.IssueTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            refreshToken.ExpirationTime = DateTimeOffset.UtcNow.Add(options.ExpirationTime)
                                                                        .ToUnixTimeSeconds();

            await userService.AddRefreshToken(user, refreshToken);

            return Result<string>.Success(refreshToken.Token);
        }

        public async Task<Result> ValidateTokenAsync(string token)
        {
            User user = await userService.FindAsync(user => user.RefreshToken.Token == token);

            if (user == null)
                return Result.Failure("Refresh token is not valid");

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (user.RefreshToken.ExpirationTime < currentTime)
                return Result.Failure("Refresh token has been expired");

            return Result.Success();
        }
    }
}
