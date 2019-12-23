using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Schoolman.Student.Infrastructure
{
    public class UserDataContext : IdentityDbContext<AppUser, AppRole, Guid>
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
            base.OnModelCreating(builder);
        }
    }

  
}