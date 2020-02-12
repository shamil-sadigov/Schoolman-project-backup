using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Helpers;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Base class for application services
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey">Primary key</typeparam>
    public abstract class ServiceBase<TEntity, TKey> : IServiceBase<TEntity, TKey>
                                             where TEntity : class, IEntity<TKey>, new()
    {
        protected readonly IRepository<TEntity> repository;
        public ServiceBase(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }


        #region Deleting

        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public virtual async Task<Result> DeleteByIdAsync(TKey id)
        {
            var entity = new TEntity() as IEntity<TKey>;
            entity.Id = id;
            repository.Context.Entry(entity).State = EntityState.Deleted;
            return await repository.SaveChangesAsync() > 0 ? true : false;

        }

        public virtual async Task<Result> DeleteAsync(TEntity entity)
        {
            return await repository.RemoveAndSaveAsync(entity);
        }

        public virtual async Task<Result> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = repository.Set.Where(predicate).AsNoTracking();
            return await DeleteRangeAsync(entities);
        }

        public virtual async Task<Result> DeleteRangeAsync(IEnumerable<TEntity> entites)
        {
            return await repository.RemoveRangeAndSaveAsync(entites);
        }


        #endregion

        #region Reading


        public virtual async Task<TEntity> FindByIdAsync(TKey id)
        {
            return await repository.Set.AsNoTracking().SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
              => await repository.Set.AsNoTracking().SingleOrDefaultAsync(expression);

        public virtual IAsyncEnumerable<TEntity> FindRangeAsync(Expression<Func<TEntity, bool>> predicate)
            => repository.Set.AsNoTracking().Where(predicate).AsAsyncEnumerable();


        #endregion

        #region Updating

        public virtual async Task<Result> UpdateAsync(TEntity entity)
        {
            //  will be changed soon sensi Update method updates all properties even if they are not changed
            return await repository.UpdateAndSaveAsync(entity);
        }

        public virtual async Task<Result> UpdateRange(IEnumerable<TEntity> entites)
        {
            return await repository.UpdateRangeAndSaveAsync(entites);
        }

        #endregion


        public virtual async Task<bool> ExistsAsync(TKey id)
        {
            return await repository.Set.AnyAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.Set.AnyAsync(predicate);
        }

        public virtual async Task<ICollection<TEntity>> ListAsync()
        {
            return await repository.Set.ToArrayAsync();
        }

    }
}
