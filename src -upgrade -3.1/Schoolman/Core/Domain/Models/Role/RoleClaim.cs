using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public Role Role { get; set; }
    }


}
