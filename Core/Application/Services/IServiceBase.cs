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
    /// Base interface for application services should be implemented by all services
    /// When deleteding and updating methods, if operation fails, false will be returned and error message will be logged
    /// So if false is returned, take a look at logs
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IServiceBase<TEntity, TKey> where TEntity : class, new()
    {
        Task<bool> ExistsAsync(TKey id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        #region Reading

        /// <summary>
        /// Finds entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="track">Should EF Core track entity or not. It's recommended to user 'false' when you only reading this entity</param>
        /// <returns></returns>
        Task<TEntity> FindByIdAsync(TKey id);
        /// <summary>
        /// Finds entity based on predicate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="track">Should EF Core track entity or not? It's recommended to use 
        /// 'false' when you only reading this entity and not going to delete or update for the sake of performance</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Find range of entities base on predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="track">Should EF Core track entity or not? It's recommended to use 
        /// 'false' when you only reading this entity and not going to delete or update for the sake of performance</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Returns all entities from DB
        /// </summary>
        /// <param name="track">Should EF Core track entity or not? It's recommended to use 
        /// 'false' when you only reading this entity and not going to delete or update for the sake of performance</param>
        /// <returns></returns>

        Task<List<TEntity>> ToListAsync(bool track = true);

        #endregion

        #region Deleting

        /// <summary>
        /// Delete entity by Id. By default if deletion fails, error message will be logged and return value will be false.
        /// If you want to get exception when deleting fails, you may specify throwOnFail=true
        /// </summary>
        /// <param name="id"></param>
        /// <param name="throwOnFail">When false, no exception will be throw during deleting entity. Set it true if you would like to get exception</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(TKey id, bool throwOnFail = false);
        /// <summary>
        /// Delete specified entity. By default if deletion fails, error message will be logged and return value will be false.
        ///  If you want to get exception when deleting fails, you may specify throwOnFail=true
        /// </summary>
        /// <param name="throwOnFail">When false, no exception will be throw during deleting entity. Set it true if you would like to get exception</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity user, bool throwOnFail = false);
        /// <summary>
        /// Delete all entites that match predicate. By default if deletion fails, error message will be logged and return value will be false.
        /// If you want to get exception when deleting fails, you may specify throwOnFail=true
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="throwOnFail">When false, no exception will be throw during deleting entity. Set it true if you would like to get exception</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool throwOnFail = false);

        /// <summary>
        /// Delete range of entities. By default if deletion fails, error message will be logged and return value will be false.
        /// If you want to get exception when deleting fails, you may specify throwOnFail=true
        /// </summary>
        /// <param name="user"></param>
        /// <param name="throwOnFail"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeAsync(IEnumerable<TEntity> user, bool throwOnFail = false);

        #endregion

        #region Updating
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="throwOnFail">When false, no exception will be throw during updating entity. Set it true if you would like to get exception</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, bool throwOnFail = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entites"></param>
        /// <param name="throwOnFail">When false, no exception will be throw during updating entity. Set it true if you would like to get exception</param>
        /// <returns></returns>
        Task<bool> UpdateRange(IEnumerable<TEntity> entites, bool throwOnFail = false);
        #endregion
    }
}
