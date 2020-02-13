using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> role)
        {
            role.ToTable("RoleClaims");

            role.HasOne(u => u.Role)
              .WithMany(u => u.Claims)
              .HasForeignKey(u => u.RoleId)
              .IsRequired();
        }
    }
}
