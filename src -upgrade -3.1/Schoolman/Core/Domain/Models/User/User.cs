using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class User:IdentityUser
    {
        #region Ctor

        public User()
        {
            _userRoleTenantRelation = new HashSet<UserRoleTenant>();
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
            RefreshToken = new RefreshToken();
        }

        #endregion

        public RefreshToken RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [NotMapped]
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var relation in _userRoleTenantRelation)
                    yield return relation.Role;
                
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



        #region Navigation Properties

        private ICollection<UserRoleTenant> _userRoleTenantRelation { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion


    }
}
