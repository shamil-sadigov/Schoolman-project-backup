using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public User User { get; set; }
    }

    public class RefreshToken
    {
        public readonly string Token;
        public long IssueTime { get; set; }
        public long ExpirationTime { get; set; }

        public RefreshToken(long issueTime, long expirationTime)
        {
            Token = Guid.NewGuid().ToString();
            IssueTime = issueTime;
            ExpirationTime = expirationTime;
        }

        public RefreshToken()
        {

        }

    }
}
