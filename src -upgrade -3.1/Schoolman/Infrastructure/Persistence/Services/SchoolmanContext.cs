﻿using Application.Services;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Services;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class SchoolmanContext : IdentityDbContext<User, Role, string,
                                                 UserClaim,
                                                 Client,
                                                 UserLogin,
                                                 RoleClaim,
                                                 UserToken>

    {
        private readonly ICurrentUserService currentUserService;

        public SchoolmanContext(DbContextOptions<SchoolmanContext> ops,
                                ICurrentUserService currentUserService) : base(ops)
        {
            this.currentUserService = currentUserService;
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


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
                switch (entry.State)
                {
                    case EntityState.Deleted when entry is IDeleteableEntity entity:
                        entity.IsDeleted = true;
                        entity.DeletedBy = currentUserService.CurrentUserId();
                        entry.State = EntityState.Modified;
                        if (entry is IAuditableEntity auditableEntity)
                            auditableEntity.LastModifiedBy = currentUserService.CurrentUserId();
                        continue;
                    case EntityState.Modified when entry is IAuditableEntity entity:
                        entity.LastModifiedBy = currentUserService.CurrentUserId();
                        continue;

                    case EntityState.Added when entry is IAuditableEntity entity:
                        entity.CreatedBy = currentUserService.CurrentUserId();
                        continue;

                    default:
                        continue;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}