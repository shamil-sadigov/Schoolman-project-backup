using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public User User { get; set; }
    }

}
