using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public User User { get; set; }
    }

}

