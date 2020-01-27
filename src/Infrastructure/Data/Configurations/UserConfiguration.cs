using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            #region Relations
            user.HasOne(u => u.RefreshToken)
                       .WithOne(r => r.User)
                       .HasForeignKey<RefreshToken>(r => r.UserId)
                       .OnDelete(DeleteBehavior.Cascade);


            user.HasMany(u => u.UserRoleCompanies)
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();



            #endregion


            #region Property configuration
            user.Property(u => u.FirstName)
                        .HasMaxLength(60);

            user.Property(u => u.LastName)
                    .HasMaxLength(60);



            user.HasIndex(u => u.Email);

            #endregion
        }
    }
}
