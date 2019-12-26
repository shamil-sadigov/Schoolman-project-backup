using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;

namespace Schoolman.Student.Infrastructure
{
    public class UserDataContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public UserDataContext(DbContextOptions<UserDataContext> ops) : base(ops)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RefreshToken>()
                   .HasKey(r => r.Token);

            builder.Entity<RefreshToken>()
                   .Property(r => r.Jti).IsRequired();

            builder.Entity<RefreshToken>()
                   .HasOne(r => r.User)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Entity<AppUser>()
                    .Property(u => u.FirstName)
                    .HasMaxLength(60);


            builder.Entity<AppUser>()
                    .Property(u => u.LastName)
                    .HasMaxLength(60);
                    

            base.OnModelCreating(builder);
        }
    }

  
}