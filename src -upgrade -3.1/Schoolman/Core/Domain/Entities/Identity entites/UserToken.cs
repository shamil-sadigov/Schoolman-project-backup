using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public User User { get; set; }
    }
}
