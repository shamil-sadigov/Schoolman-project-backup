using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> customer)
        {
            customer.HasKey(x => x.Id);

            customer.Property(x => x.Id)
                           .ValueGeneratedOnAdd();

            customer.HasOne(x => x.Company)
                           .WithMany(c=> c.Customers)
                           .HasForeignKey(urt => urt.CompanyId)
                           .OnDelete(DeleteBehavior.Restrict);

            customer.HasOne(urt => urt.Role)
                           .WithMany(r => r.Customers)
                           .HasForeignKey(urt => urt.RoleId)
                           .OnDelete(DeleteBehavior.Restrict);

            customer.HasOne(urt => urt.UserInfo)
                           .WithMany(u=> u.Customers)
                           .HasForeignKey(urt => urt.UserId)
                           .OnDelete(DeleteBehavior.Cascade);


            customer.OwnsOne(model => model.RefreshToken, token =>
            {
                token.Property(rt => rt.Token).HasMaxLength(256);
                token.ToTable("RefreshTokens");
            });

            customer.HasIndex(x => new { x.UserId, x.RoleId, x.CompanyId })
                            .IsUnique();

            customer.ToTable("Customers");
        }
    }
}
