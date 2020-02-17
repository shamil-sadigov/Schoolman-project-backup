using Application.Services;
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
        private readonly ICustomerManager customerManager;
        private readonly IRepository<RefreshToken> repository;

        public RefreshTokenService(ICustomerManager customerManager,
                                   IOptionsMonitor<RefreshTokenOptions> optionsMonitor,
                                   IRepository<RefreshToken> repository)
        {
            this.options = optionsMonitor.CurrentValue;
            this.customerManager = customerManager;
            this.repository = repository;
        }


        public async Task<Result<string>> GenerateTokenAsync(Customer client)
        {
            var refreshToken = await repository.FindAsync(x => x.CustomerId == client.Id);

            refreshToken ??= new RefreshToken();

            //refreshToken.IssueTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //refreshToken.ExpirationTime = DateTimeOffset.UtcNow.Add(options.ExpirationTime)
            //                                                            .ToUnixTimeSeconds();


            // to delete
            refreshToken.IssueTime = 1313;
            refreshToken.ExpirationTime = 1313;




            refreshToken.CustomerId = client.Id;

            await repository.AddOrUpdateAsync(refreshToken);

            // to delete
            //await customerManager.AddRefreshTokenAsync(client, refreshToken);

            return Result<string>.Success(refreshToken.Token);
        }



        public async Task<Result> ValidateTokenAsync(string token)
        {
            Customer customer = await customerManager.FindAsync(c => c.RefreshToken.Token == token);

            if (customer == null)
                return Result.Failure("Refresh token is not valid");

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (customer.RefreshToken.ExpirationTime < currentTime)
                return Result.Failure("Refresh token has been expired");

            return Result.Success();
        }
    }
}
