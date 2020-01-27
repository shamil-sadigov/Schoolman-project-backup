using Domain.Models;
using System.Collections.Generic;

namespace Domain
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tenant()
        {
            UserRoleTenants = new HashSet<UserRoleTenant>();
        }
        public ICollection<UserRoleTenant> UserRoleTenants { get; set; }
    }
}
