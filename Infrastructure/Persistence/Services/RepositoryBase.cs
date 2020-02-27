using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class RepositoryBase<T> : IRepository<T> where T : class, new()
    {
        private readonly SchoolmanContext context;
        private readonly DbSet<T> set;
        public RepositoryBase(SchoolmanContext context)
        {
            this.context = context;
            set = context.Set<T>();
        }

        public DbContext Context { get => context; }

        public async Task AddOrUpdateAsync(T entity)
        {
            EnsureEntityHasId<T>();
            var entityId = context.Entry(entity).Property("Id").CurrentValue;

            //if (entityId is null)
            //    set.Add(entity);

            if (!context.Entry(entity).IsKeySet)
                set.Add(entity);
            else
            {   // if entity is tracked, it will catched from cache
                var dbEntity = await set.FindAsync(entityId);
                context.Entry(dbEntity).CurrentValues.SetValues(entity);
                context.Entry(dbEntity).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await set.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(params T[] entities)
        {
            await set.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            set.Remove(entity);
            await context.SaveChangesAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => set.AnyAsync(predicate);

        public IQueryable<T> AsQueryable()
             => set.AsQueryable();

        public async Task RemoveRangeAsync(params T[] entities)
        {
            set.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
        public async Task<T> FindAsync(object key)
            => await set.FindAsync(key);
        public Task<T> FindAsync(Expression<Func<T, bool>> predicate)
            => set.Where(predicate).FirstOrDefaultAsync();
        public IQueryable<T> FindRange(Expression<Func<T, bool>> predicate)
             => set.Where(predicate);
        private void EnsureEntityHasId<Tentity>()
        {
            // ensure entity has Id
            if (typeof(Tentity).GetProperty("Id") == null)
                throw new DbUpdateException("Updating entity has no Id");
        }
    }
}