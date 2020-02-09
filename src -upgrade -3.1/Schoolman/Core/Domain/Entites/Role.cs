using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Role : IdentityRole, IEntity<string>
    {
        public Role(string roleName)
        {
            Name = roleName;
            clients = new HashSet<AppClient>();
        }


        public Role() { }

        [NotMapped]
        public IEnumerable<User> Users
        {
            get
            {
                foreach (var client in clients)
                    yield return client.User;
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


        private ICollection<AppClient> clients;
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
