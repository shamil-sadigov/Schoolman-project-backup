using Microsoft.AspNetCore.Identity;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Schoolman.Student.Infrastructure
{
    public class Role:IdentityRole
    {
        public ICollection<UserRoleTenant> UserRoleCompanies { get; set; }
    }
}