using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Helpers
{
    public static class AuthExtensions
    {
        public static async Task<(string jwt, string refresh)> GenerateTokensAsync<Tuser> 
            (this IAuthTokenManager<Tuser> tokenManager , Tuser user) where Tuser: class
        {
            var jwt = tokenManager.GenerateJWT(user);
            var refreshToken = await tokenManager.GenerateRefreshTokenAsync(jwt);
            return (jwt, refreshToken);
        } 
    }
}
