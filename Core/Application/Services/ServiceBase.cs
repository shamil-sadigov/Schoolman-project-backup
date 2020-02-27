using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Base class for application services
    /// </summary>s
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey">Primary key</typeparam>
    public abstract class ServiceBase<TEntity, TKey> : IServiceBase<TEntity, TKey>
                                             where TEntity : class, IEntity<TKey>, new()
    {
        protected readonly IRepository<TEntity> repository;
        protected readonly ILogger logger;


        public ServiceBase(IRepository<TEntity> repository,
                           ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }


        #region Deleting

        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public virtual async Task<bool> DeleteByIdAsync(TKey id, bool throwOnFail)
        {
            var entity = new TEntity() as IEntity<TKey>;
            entity.Id = id;

            try
            {
                await repository.RemoveAsync((TEntity)entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ServiceBase. Unable to delete entity {nameof(TEntity)}: " +
                                    "Db Exception thrown while deleting entity {@entity} with Id {@id}", entity, id);

                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity, bool throwOnFail)
        {
            try
            {
                await repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ServiceBase. Unable to delete entity {typeof(TEntity)}: " +
                                    "Db Exception thrown while deleting entity {@entity} ", entity);

                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;

        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool throwOnFail)
        {
            var entities = repository.FindRange(predicate);

            try
            {
                await repository.RemoveRangeAsync(entities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ServiceBase. Unable to delete entities {entities.GetType()}." +
                                    "Db Exception thrown while deleting entities {@entities} ", entities);

                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public virtual async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, bool throwOnFail)
        {

            try
            {
                await repository.RemoveRangeAsync(entities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ServiceBase. Unable to delete entities {entities.GetType()}." +
                                    "Db Exception thrown while updating entity {@entities}", entities);

                if (throwOnFail)
                    throw ex;

                return false;
            }
            return true;
        }

        #endregion

        #region Reading

        public virtual Task<TEntity> FindByIdAsync(TKey id)
            => repository.FindAsync(id);

        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
                => repository.FindAsync(predicate);

        public virtual async Task<IEnumerable<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> predicate)
                => await repository.FindRange(predicate).ToListAsync();


        #endregion

        #region Updating

        public virtual async Task<bool> UpdateAsync(TEntity entity, bool throwOnFail)
        {
            try
            {
                await repository.AddOrUpdateAsync(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ServiceBase. Unable to update entity: {entity.GetType()}" +
                                    "Db Exception thrown while updating entity {@entity}", entity);

                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public virtual async Task<bool> UpdateRange(IEnumerable<TEntity> entites, bool throwOnFail)
        {
#warning UpdateRange in ServiceBase is not implemented

            // will be implemented soon
            // no worries   
            throw new NotImplementedException();
        }

        #endregion

        public virtual async Task<bool> ExistsAsync(TKey id)    
            => await repository.AnyAsync(e => e.Id.Equals(id));
        
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await repository.AnyAsync(predicate);

        public virtual async Task<List<TEntity>> ToListAsync(bool track)
            => await repository.AsQueryable().ToListAsync();
                    
    }
}
