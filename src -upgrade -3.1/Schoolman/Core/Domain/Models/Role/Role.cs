using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Role : IdentityRole
    {
        public Role()
        {
            _userRoleTenantRelation = new HashSet<UserRoleTenant>();
        }

        [NotMapped]
        public IEnumerable<User> Users
        {
            get
            {
                foreach (var relation in _userRoleTenantRelation)
                    yield return relation.User;
            }
        }

        [NotMapped]
        public IEnumerable<Tenant> Tenants
        {
            get
            {
                foreach (var relation in _userRoleTenantRelation)
                    yield return relation.Tenant;
            }
        }


        private ICollection<UserRoleTenant> _userRoleTenantRelation { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
