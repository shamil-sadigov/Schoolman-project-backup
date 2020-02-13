﻿using Domain;
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
    /// </summary>
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
                repository.Context.Entry(entity).State = EntityState.Deleted;
                await repository.SaveChangesAsync();
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
                repository.Remove(entity);
                await repository.SaveChangesAsync();
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
            var entities = repository.FindArrange(predicate);

            try
            {
                repository.RemoveRange(entities);
                await repository.SaveChangesAsync();
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
                repository.RemoveRange(entities);
                await repository.SaveChangesAsync();
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
        {
            return repository.FindAsync(id);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
              => await repository.FindAsync(expression);

        public virtual IEnumerable<TEntity> FindRangeAsync(Expression<Func<TEntity, bool>> predicate)
            =>  repository.FindArrange(predicate);


        #endregion

        #region Updating

        public virtual async Task<bool> UpdateAsync(TEntity entity, bool throwOnFail)
        {
            try
            {
                repository.Update(entity);
                await repository.SaveChangesAsync();
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
        {
            return await repository.AnyAsync(predicate);
        }

        public virtual IEnumerable<TEntity> ToEnumerable()
            => repository.AsNoTracking();


        public virtual async Task<List<TEntity>> ToList()
            => await repository.AsNoTracking().ToListAsync();

       
    }
}