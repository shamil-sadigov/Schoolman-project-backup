using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Core.Domain.Domain
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRoleTenant> UserRoleCompanies { get; set; }
    }
}
