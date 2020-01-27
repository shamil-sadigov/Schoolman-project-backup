using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schoolman.Student.Infrastructure
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }

        #region Ctor-s

        public User()
        {

        }

        public User(string email, string userName)
        {
            this.Email = email;
            this.UserName = userName;
        }


        #endregion

        #region Navigation properties

        public virtual ICollection<UserRoleTenant> UserRoleCompanies { get; set; }
        //public virtual ICollection<UserCompany> Companies { get; set; }


        public Guid RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; } 
        #endregion
    }
}