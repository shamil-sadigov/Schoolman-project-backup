using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    public class Role : IdentityRole
    {
        public Role()
        {
            UserRoleTenants = new HashSet<UserRoleTenant>();
        }
        public ICollection<UserRoleTenant> UserRoleTenants { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
