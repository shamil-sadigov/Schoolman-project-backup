using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> client)
        {
            client.HasKey(model => model.Id);

            client.Property(model => model.Id)
                           .ValueGeneratedOnAdd();

            client.HasOne(urt => urt.Company)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.CompanyId)
                           .OnDelete(DeleteBehavior.Restrict);

            client.HasOne(urt => urt.Role)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.RoleId)
                           .OnDelete(DeleteBehavior.Restrict);

            client.HasOne(urt => urt.User)
                           .WithMany("clients")
                           .HasForeignKey(urt => urt.UserId)
                           .OnDelete(DeleteBehavior.Cascade);

            client.OwnsOne(model => model.RefreshToken, rt =>
            {
                rt.Property(rt => rt.Token).HasMaxLength(256);
                rt.ToTable("RefreshTokens");
            });

            client.HasIndex(model => new { model.UserId, model.RoleId, model.CompanyId })
                           .IsUnique();

            client.ToTable("AppClients");
        }
    }
}
