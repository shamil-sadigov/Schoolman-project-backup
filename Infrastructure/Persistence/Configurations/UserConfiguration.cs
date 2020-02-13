using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : ConfigurationBase<User, string>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(model => model.UserName).HasMaxLength(50);
            builder.Property(model => model.NormalizedUserName).HasMaxLength(50);
            builder.Property(model => model.Email).HasMaxLength(100);
            builder.Property(model => model.NormalizedEmail).HasMaxLength(100);
            builder.Property(model => model.Id).ValueGeneratedOnAdd();
            builder.Property(model => model.PhoneNumber).HasMaxLength(50);
            builder.Property(model => model.FirstName).HasMaxLength(50);
            builder.Property(model => model.LastName).HasMaxLength(50);

            base.Configure(builder);
        }
    }
}
