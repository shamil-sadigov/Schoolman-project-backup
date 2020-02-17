using System;

namespace Domain.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
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
            Token = Guid.NewGuid().ToString();
        }


        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
