using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> user)
        {
            user.ToTable("UserLogins");
            user.HasOne(u => u.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        }
    }
}
