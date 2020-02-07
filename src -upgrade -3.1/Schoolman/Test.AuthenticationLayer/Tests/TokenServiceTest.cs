using Application.Common.Models;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.AuthenticationLayer
{
    public class TokenServiceTest:BasicTest
    {
        private readonly IAuthTokenService tokenService;
        private readonly IUserService userService;
   



        public TokenServiceTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory)
        {
            tokenService = factory.Services.GetRequiredService<IAuthTokenService>();
            userService = factory.Services.GetRequiredService<IUserService>();
        }




        [Fact(DisplayName = "TokenService.GenerateTokenAsync() => Doesn't generate token for non existing user")]
        public async Task TokenService_Doesnt_GenerateTokens_For_NonExisting_User()
        {
            Result<AuthenticationCredential> credentials 
                = await tokenService.GenerateAuthenticationTokensAsync("Not existing Id");

            Assert.False(credentials.Succeeded, "TokenService should not created token since no user existed with such Id");
        }





        [Fact(DisplayName = "TokenService.GenerateTokenAsync() => Generates token for existing user")]
        public async Task TokenService_GenerateTokenAsync_For_User()
        {
            bool repeated = false;

        // Check if we can generate token for 

            User user = await CreateNewUser();

            Assert.True(user!=null, "User should be created");

        Label_For_Repeat_Token_Generation:

            #region Generate tokens and Assert generation succeeded

            // Act
            Result<AuthenticationCredential> tokenGenerationResult = 
                await tokenService.GenerateAuthenticationTokensAsync(user.Id);

            // Assertion
            Assert.True(tokenGenerationResult.Succeeded, "TokenService should generate tokens for user");

            #endregion

            #region Check if refresh token is persisted in DB

            string refreshToken = tokenGenerationResult.Response.RefresthToken;

            bool tokenIsPersistedInDb = await userService.UserRepository.Collection
                                            .AnyAsync(u => u.RefreshToken.Token == refreshToken && u.Id == user.Id);

            Assert.True(tokenIsPersistedInDb, "Refresh token should be persisted in DB");

            // Just try to generate token 2 time for the same user
            if (!repeated)
            {
                repeated = true; // if token was generated only once, the go to generate it second time
                goto Label_For_Repeat_Token_Generation;
            }

            #endregion
        }







        [Fact(DisplayName = "TokenService.RefreshTokensAsync() => Doesn't refresh  tokens that are not expired")]
        public async Task TokenService_GenerateTokenAsync_For_User2()
        {
            #region Create user and Assert creation


            User user = await CreateNewUser();

            Assert.True(user != null, "User should be created");



            #endregion

            #region Generate tokens and Assert generation succeeded

            // Act
            Result<AuthenticationCredential> tokenGenerationResult = await tokenService.GenerateAuthenticationTokensAsync(user.Id);

            // Assertion
            Assert.True(tokenGenerationResult.Succeeded, "TokenService should generate tokens for user");


            #endregion



           bool tokensRefreshed = await tokenService.ExchangeAuthenticationTokensAsync
                                         (acessToken: tokenGenerationResult.Response.AccessToken,
                                        refreshToken: tokenGenerationResult.Response.RefresthToken);


            Assert.False(tokensRefreshed, "Tokens should not be refreshed since they are not supposed to be expired");
        }



        private async Task<Result<User>> CreateNewUser()
        {
            string email = $"{Guid.NewGuid().ToString()}@example.com";

            return await userService.CreateUserAsync(user: new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email,
            },
            password: "Qwerty1414@");
        }





    }
}
