using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> refreshToken)
        {
            refreshToken.ToTable("refresh_tokens");
            refreshToken.HasKey(x => x.Id);
            refreshToken.Property(x => x.Id).ValueGeneratedOnAdd();

            refreshToken.HasOne(x => x.Customer)
                        .WithOne(c => c.RefreshToken)
                        .HasForeignKey<RefreshToken>(r => r.CustomerId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}