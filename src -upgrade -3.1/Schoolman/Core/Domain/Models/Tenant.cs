using Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tenant()
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
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var relation in _userRoleTenantRelation)
                    yield return relation.Role;

            }
        }

        private ICollection<UserRoleTenant> _userRoleTenantRelation { get; set; }
    }
}
