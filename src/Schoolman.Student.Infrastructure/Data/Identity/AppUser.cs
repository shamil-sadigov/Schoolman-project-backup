using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolman.Student.Infrastructure
{
    public class AppUser:IdentityUser<Guid>
    {
        public AppUser()
        {

        }
        public AppUser(string email, string userName)
        {
            this.Email = email;
            this.UserName = userName;
        }
    }

}