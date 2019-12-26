using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolman.Student.Infrastructure
{
    public class AppUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }
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