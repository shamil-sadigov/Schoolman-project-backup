using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly SchoolmanContext context;
        private readonly DbSet<T> set;
        public RepositoryBase(SchoolmanContext context)
        {
            this.context = context;
            set = context.Set<T>();
        }

        public DbContext Context { get => context; }



        public Task<int> SaveChangesAsync()
            => context.SaveChangesAsync();


        public async Task AddAsync(T entity)
           => await set.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await set.AddRangeAsync(entities);

        public void Remove(T entity)
            => set.Remove(entity);

        public  void Update(T entity)
        {
            var updatingEntityId = context.Entry(entity).Property("Id").CurrentValue;

            T trackedEntity = set.Local.FirstOrDefault(trackedEntity =>
            {
                var updadingEntityId = context.Entry(entity).Property("Id").CurrentValue;
                var trackedEntityId = context.Entry(trackedEntity).Property("Id").CurrentValue;

                return updadingEntityId.Equals(trackedEntityId);
            });

            if (trackedEntity != null)
                context.Entry(trackedEntity).State = EntityState.Detached;

            context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> AsNoTracking()
            => set.AsNoTracking();

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => set.AnyAsync(predicate);

        public Task AddRangeAsync(params T[] entities)
            => set.AddRangeAsync(entities);
        

        public IQueryable<T> AsQueryable()
             => set.AsQueryable();
        
        public void RemoveRange(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindAsync(object key)
        {
            return await set.FindAsync(key);
        }

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate)
            => set.AsNoTracking().Where(predicate).FirstOrDefaultAsync();

        public IEnumerable<T> FindArrange(Expression<Func<T, bool>> predicate)
        {
            return set.AsNoTracking().Where(predicate);
        }
    }
}
