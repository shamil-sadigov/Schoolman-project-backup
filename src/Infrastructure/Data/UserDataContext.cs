using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Schoolman.Student.Infrastructure.Data.Identity;
using System;
using System.Linq;
using System.Reflection;

namespace Schoolman.Student.Infrastructure
{
    public class UserDataContext : IdentityDbContext<User, Role, string,
                                                     IdentityUserClaim<string>,
                                                     UserRoleTenant,
                                                     IdentityUserLogin<string>,
                                                     IdentityRoleClaim<string>,
                                                     IdentityUserToken<string>>
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
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}