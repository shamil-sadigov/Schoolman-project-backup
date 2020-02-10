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
            Customers = new HashSet<Customer>();
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
        }

        #endregion

        // Owned entity
       
        public string FirstName { get; set; }
        public string LastName { get; set; }


      

        #region Navigation Properties

        public ICollection<Customer> Customers { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion
    }
}
