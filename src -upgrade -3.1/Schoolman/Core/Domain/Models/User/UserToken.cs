using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public User User { get; set; }
    }

    public class RefreshToken
    {




        public string Token { get; set; }
        public string AccessTokenId { get; set; }

        public long IssueTime { get; set; }
        public long ExpirationTime { get; set; }

    }
}
