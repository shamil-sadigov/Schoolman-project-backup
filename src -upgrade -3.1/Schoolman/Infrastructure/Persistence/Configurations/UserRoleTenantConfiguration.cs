using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class UserRoleTenantConfiguration : IEntityTypeConfiguration<UserRoleCompany>
    {
        public void Configure(EntityTypeBuilder<UserRoleCompany> userRoleTenant)
        {
            userRoleTenant.Property(model => model.ClientId)
                          .ValueGeneratedOnAdd()
                          .IsRequired();


            userRoleTenant.HasIndex(model => model.ClientId);

            userRoleTenant.HasOne(urt => urt.Company)
                          .WithMany("_userRoleTenantRelation")
                          //.WithMany(t => t.UserRoleTenantRelation)
                          .HasForeignKey(urt => urt.Company)
                          .OnDelete(DeleteBehavior.Restrict);

            userRoleTenant.HasOne(urt => urt.Role)
                          .WithMany("_userRoleTenantRelation")
                         //.WithMany(t => t.UserRoleTenantRelation)
                         .HasForeignKey(urt => urt.RoleId)
                         .OnDelete(DeleteBehavior.Restrict);

            userRoleTenant.HasOne(urt => urt.User)
                          //.WithMany(t => t.UserRoleTenantRelation)
                          .WithMany("_userRoleTenantRelation")
                         .HasForeignKey(urt => urt.UserId)
                         .OnDelete(DeleteBehavior.Cascade);

            userRoleTenant.HasKey(model => new { model.UserId, model.RoleId, model.Company });

            userRoleTenant.ToTable("UserRoleTenantRelations");
        }
    }
}
