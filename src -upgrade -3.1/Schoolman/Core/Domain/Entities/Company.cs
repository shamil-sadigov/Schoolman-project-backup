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
            Customers = new HashSet<Customer>();
        }

      
        public ICollection<Customer> Customers { get; set; }

    }
}
