using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class SchoolmanContext : IdentityDbContext<User, Role, string,
                                                 UserClaim,
                                                 UserRoleCompany,
                                                 UserLogin,
                                                 RoleClaim,
                                                 UserToken>
    {

        public SchoolmanContext(DbContextOptions<SchoolmanContext> ops) : base(ops)
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
