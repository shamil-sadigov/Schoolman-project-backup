using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// Base class for User class
    /// </summary>
    public abstract class UserBase:IdentityUser<string>, 
                                       IEntityBase<string>
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
    }


}
