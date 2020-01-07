using Schoolman.Student.Infrastructure.AuthOptions;
using System;

namespace Schoolman.Student.Infrastructure.Data.Identity
{
    public class RefreshToken
    {
        public string Token { get; set; } 
        public string Jti { get; set; }
        /// <summary>
        /// Unix format
        /// </summary>
        public long Creation_time { get; set; }
        public long Expiration_time { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public static RefreshToken NewRefreshToken(string jti, string userId, TimeSpan expirationTime)
        {
            return new RefreshToken()
            {
                Jti = jti,
                Creation_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserId = userId,
                Expiration_time = DateTimeOffset.UtcNow.Add(expirationTime).ToUnixTimeSeconds(),
                Token = Guid.NewGuid().ToString()
            };
        }
    }
}
