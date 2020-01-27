using Schoolman.Student.Infrastructure.AuthOptions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolman.Student.Infrastructure.Data.Identity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        [NotMapped]
        public string Token { get => Id.ToString(); }
        public string Jti { get; set; }
        /// <summary>
        /// Unix format
        /// </summary>
        public long Creation_time { get; set; }
        public long Expiration_time { get; set; }


        public string UserId { get; set; }
        public virtual User User { get; set; }

        public static RefreshToken NewRefreshToken(string jti, string userId, TimeSpan expirationTime)
        {
            return new RefreshToken()
            {
                Jti = jti,
                Creation_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserId = userId,
                Expiration_time = DateTimeOffset.UtcNow.Add(expirationTime).ToUnixTimeSeconds(),
                Id = Guid.NewGuid()
            };
        }
    }
}
