using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public User User { get; set; }
    }


}
