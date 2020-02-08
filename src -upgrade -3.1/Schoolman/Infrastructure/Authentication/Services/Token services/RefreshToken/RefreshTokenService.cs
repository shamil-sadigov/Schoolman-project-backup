using Application.Services.Token;
using Domain;
using Domain.Models;
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

        public RefreshTokenService(IUserService userService)
        {
            this.userService = userService;
        }


        public async Task<Result<string>> GenerateTokenAsync(RefreshTokenCreationParameters parameters)
        {
            var refreshToken = new RefreshToken();
            refreshToken.IssueTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            refreshToken.ExpirationTime = DateTimeOffset.UtcNow.Add(parameters.Options.ExpirationTime)
                                                                        .ToUnixTimeSeconds();

            await userService.UpdateRefreshTokenAsync(parameters.User, refreshToken);

            return Result<string>.Success(refreshToken.Token);
        }

        public async Task<Result> ValidateTokenAsync(RefreshTokenValidationParameters token)
        {
            User user = await userService.FindAsync(user => user.RefreshToken.Token == token.RefreshToken);

            if (user == null)
                return Result.Failure("Refresh token is not valid");

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (user.RefreshToken.ExpirationTime < currentTime)
                return Result.Failure("Refresh token has been expired");

            return Result.Success();
        }
    }
}
