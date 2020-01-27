using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Data.Identity
{
    public class UserRoleTenant: IdentityUserRole<string>
    {
        public User User { get; set; }
        public Role Role { get; set; }
        public Tenant Tenant { get; set; }

        public string TenantId { get; set; }
    }
}
