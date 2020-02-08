using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Many-to-many table
    /// </summary>
    public class UserRoleCompany:IdentityUserRole<string>
    {
        public string ClientId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }

        public string CompanyId { get; set; }
    }
}
