using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class UserRoleTenantConfiguration : IEntityTypeConfiguration<UserRoleTenant>
    {
        public void Configure(EntityTypeBuilder<UserRoleTenant> userRoleTenant)
        {
            userRoleTenant.HasOne(urt => urt.Tenant)
                          .WithMany(t => t.UserRoleTenants)
                          .HasForeignKey(urt => urt.TenantId)
                          .OnDelete(DeleteBehavior.SetNull);

            userRoleTenant.HasOne(urt => urt.Role)
                         .WithMany(t => t.UserRoleTenants)
                         .HasForeignKey(urt => urt.RoleId)
                         .OnDelete(DeleteBehavior.SetNull);

            userRoleTenant.HasOne(urt => urt.User)
                         .WithMany(t => t.UserRoleTenants)
                         .HasForeignKey(urt => urt.UserId)
                         .OnDelete(DeleteBehavior.Cascade);

            userRoleTenant.HasKey(model => new { model.UserId, model.RoleId, model.TenantId });

            userRoleTenant.ToTable("UserRoleTenants");
        }
    }
}
