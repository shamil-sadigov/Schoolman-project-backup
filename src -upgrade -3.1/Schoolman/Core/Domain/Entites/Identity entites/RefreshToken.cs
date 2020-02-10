using System;

namespace Domain.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
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
