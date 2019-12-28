using Schoolman.Student.Infrastructure.AuthOptions;
using System;

namespace Schoolman.Student.Infrastructure.Data.Identity
{
    public class RefreshToken
    {
        public string Token { get; set; } 
        public string Jti { get; set; }
        public DateTime Created { get; set; }
        public DateTimeOffset Expires { get; set; }
        public RefreshToken()
        {
            Token = Guid.NewGuid().ToString();
        }


        public string UserId { get; set; }
        public AppUser User { get; set; }

        public static RefreshToken NewRefreshToken(string jti, string userId, RefreshTokenOptions refreshOptions)
        {
            return new RefreshToken()
            {
                Jti = jti,
                Created = DateTime.UtcNow,
                UserId = userId,
                Expires = DateTime.UtcNow.Add(refreshOptions.ExpirationTime)
            };
        }
    }
}
