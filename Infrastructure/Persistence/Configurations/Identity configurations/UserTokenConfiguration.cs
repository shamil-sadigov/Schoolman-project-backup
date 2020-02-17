using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> user)
        {
            user.ToTable("user_tokens");
            user.Property(model => model.Name).HasMaxLength(50);

            user.HasOne(u => u.User)
              .WithMany(u => u.Tokens)
              .HasForeignKey(u => u.UserId)
              .IsRequired();
        }
    }
}
