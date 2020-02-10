using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Role : RoleBase
    {
        public Role(string roleName)
        {
            Name = roleName;
            Customers = new HashSet<Customer>();
        }


        public Role() { }

        public  ICollection<Customer> Customers { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
    }
}
