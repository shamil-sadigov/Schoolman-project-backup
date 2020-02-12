using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class ClaimsException:Exception
    {
        public Customer Customer { get; set; }

        public ClaimsException(string message, Customer customer) : base(message)
        {
            Customer = customer;
        }
    }
}
