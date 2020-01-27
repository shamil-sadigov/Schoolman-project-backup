using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Schoolman.Student.Infrastructure.Data.Identity;

namespace Schoolman.Student.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> refreshToken)
        {
            refreshToken.HasIndex(r => r.UserId);
            refreshToken.Property(r => r.Jti).IsRequired();
        }
    }
}
