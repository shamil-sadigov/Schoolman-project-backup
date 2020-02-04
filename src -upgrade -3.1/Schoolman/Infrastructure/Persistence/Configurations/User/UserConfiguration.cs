using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.ToTable("Users");
            user.Property(model => model.UserName).HasMaxLength(50);
            user.Property(model => model.NormalizedUserName).HasMaxLength(50);
            user.Property(model => model.Email).HasMaxLength(100);
            user.Property(model => model.NormalizedEmail).HasMaxLength(100);
            user.Property(model => model.Id).ValueGeneratedOnAdd();
            user.Property(model => model.PhoneNumber).HasMaxLength(50);
            user.Property(model => model.FirstName).HasMaxLength(50);
            user.Property(model => model.LastName).HasMaxLength(50);
            user.OwnsOne(model => model.RefreshToken, rt=> 
            {
                rt.Property(rt => rt.Token).HasMaxLength(256);
                rt.ToTable("RefreshTokens");
            });
        }
    }
}
