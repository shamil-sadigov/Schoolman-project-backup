using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class UserClaim:IdentityUserClaim<string>
    {
        public User User { get; set; }
    }
}
