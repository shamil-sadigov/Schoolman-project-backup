using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Many-to-many table
    /// </summary>
    public class UserRoleTenant:IdentityUserRole<string>
    {

        public User User { get; set; }
        public Role Role { get; set; }
        public Tenant Tenant { get; set; }

        public string TenantId { get; set; }
    }
}
