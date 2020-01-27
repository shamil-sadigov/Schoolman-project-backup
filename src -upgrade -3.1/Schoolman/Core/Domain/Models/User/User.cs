using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    public class User:IdentityUser
    {
        #region Ctor

        public User()
        {
            UserRoleTenants = new HashSet<UserRoleTenant>();
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
        }

        #endregion


        public RefreshToken RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public ICollection<UserRoleTenant> UserRoleTenants { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }
    }
}
