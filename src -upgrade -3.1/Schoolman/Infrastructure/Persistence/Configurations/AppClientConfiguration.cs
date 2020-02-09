using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class AppClientConfiguration : IEntityTypeConfiguration<AppClient>
    {
        public void Configure(EntityTypeBuilder<AppClient> userRoleCompany)
        {
            userRoleCompany.HasKey(model => model.Id);

            userRoleCompany.Property(model => model.Id)
                           .ValueGeneratedOnAdd();

            userRoleCompany.HasOne(urt => urt.Company)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.CompanyId)
                           .OnDelete(DeleteBehavior.Restrict);

            userRoleCompany.HasOne(urt => urt.Role)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.RoleId)
                           .OnDelete(DeleteBehavior.Restrict);

            userRoleCompany.HasOne(urt => urt.User)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.UserId)
                           .OnDelete(DeleteBehavior.Cascade);

            userRoleCompany.HasIndex(model => new { model.UserId, model.RoleId, model.CompanyId })
                           .IsUnique();

            userRoleCompany.ToTable("AppClients");
        }
    }
}
