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
            clients = new HashSet<Client>();
        }

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
        public IEnumerable<Role> Roles
        {
            get
            {
                foreach (var client in clients)
                    yield return client.Role;

            }
        }

        private ICollection<Client> clients;

    }
}
