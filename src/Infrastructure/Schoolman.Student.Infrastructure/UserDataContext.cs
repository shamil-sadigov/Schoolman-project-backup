using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Schoolman.Student.Infrastructure
{
    public class UserDataContext : IdentityDbContext<AppUser, AppRole, string>
    {

        public UserDataContext(DbContextOptions<UserDataContext> ops) : base(ops)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>();
            builder.Entity<AppRole>();
 
            base.OnModelCreating(builder);
        }
    }

  
}