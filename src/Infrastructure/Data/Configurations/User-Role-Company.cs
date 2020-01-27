using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleTenant>
    {
        public void Configure(EntityTypeBuilder<UserRoleTenant> urc)
        {
            //urc.HasOne(model => model.User)
            //        .WithMany()
            //        .HasForeignKey(model => model.UserId);


            //urc.HasOne(model => model.Role)
            //        .WithMany()
            //        .HasForeignKey(model => model.RoleId);


            //urc.HasOne(model => model.Company)
            //       .WithMany()
            //       .HasForeignKey(model => model.CompanyId);

        }
    }
}
