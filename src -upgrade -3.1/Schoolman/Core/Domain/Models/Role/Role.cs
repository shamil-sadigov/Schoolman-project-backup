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
            _userRoleCompany = new HashSet<UserRoleCompany>();
        }

        [NotMapped]
        public IEnumerable<User> Users
        {
            get
            {
                foreach (var relation in _userRoleCompany)
                    yield return relation.User;
            }
        }

        [NotMapped]
        public IEnumerable<Company> Tenants
        {
            get
            {
                foreach (var relation in _userRoleCompany)
                    yield return relation.Company;
            }
        }


        private ICollection<UserRoleCompany> _userRoleCompany { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
