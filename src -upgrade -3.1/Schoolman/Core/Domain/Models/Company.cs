using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Company:EntityBase<string>
    {
        public string Name { get; set; }

        public Company()
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
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var relation in _userRoleCompany)
                    yield return relation.Role;

            }
        }

        private ICollection<UserRoleCompany> _userRoleCompany { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}
