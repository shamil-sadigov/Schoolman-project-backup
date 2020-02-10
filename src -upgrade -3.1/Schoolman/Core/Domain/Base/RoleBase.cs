using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain
{
    public abstract class RoleBase : IdentityRole<string>,
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
