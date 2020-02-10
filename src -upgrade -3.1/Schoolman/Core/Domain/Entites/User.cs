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
            clients = new HashSet<Client>();
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
        }

        #endregion

        // Owned entity
       
        public string FirstName { get; set; }
        public string LastName { get; set; }


        [NotMapped]
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var client in clients)
                    yield return client.Role;
            }
        }

        [NotMapped]
        public IEnumerable<Company> Companies
        {
            get
            {
                foreach (var client in clients)
                    yield return client.Company;

            }
        }





        #region Navigation Properties

        private ICollection<Client> clients;
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion
    }
}
