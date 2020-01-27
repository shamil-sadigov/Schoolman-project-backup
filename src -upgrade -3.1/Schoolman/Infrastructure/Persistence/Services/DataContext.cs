using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Persistence.Contexts
{
    public class DataContext:IdentityDbContext<User,Role, string,
                                                 UserClaim,
                                                 UserRoleTenant,
                                                 UserLogin,
                                                 RoleClaim,
                                                 UserToken>
    {

        public DataContext(DbContextOptions<DataContext> ops):base(ops)
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
