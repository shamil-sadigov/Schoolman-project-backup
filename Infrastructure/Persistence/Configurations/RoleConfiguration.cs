using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> role)
        {
            role.ToTable("roles");
            role.Property(r => r.Name).HasMaxLength(50);
            role.Property(r => r.NormalizedName).HasMaxLength(50);
            role.Property(r => r.Id).ValueGeneratedOnAdd();
        }
    }
}