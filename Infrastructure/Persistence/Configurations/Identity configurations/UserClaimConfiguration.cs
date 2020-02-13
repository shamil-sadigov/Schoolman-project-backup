using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> userInfo)
        {
            userInfo.ToTable("UserClaims");
            userInfo.HasOne(u => u.User)
              .WithMany(u => u.Claims)
              .HasForeignKey(u => u.UserId)
              .IsRequired();

        }
    }
}
