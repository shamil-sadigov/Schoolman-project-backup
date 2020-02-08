using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class User : UserBase
    {
        #region Ctor

        public User()
        {
            _userRoleCompany = new HashSet<UserRoleCompany>();
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
            RefreshToken = new RefreshToken();
        }

        #endregion

        // Owned entity
        public RefreshToken RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var relation in _userRoleCompany)
                    yield return relation.Role;
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


        #region Navigation Properties

        private ICollection<UserRoleCompany> _userRoleCompany { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion
    }
}
