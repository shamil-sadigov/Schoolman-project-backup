using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Base interface for application services should be implemented by all services
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IServiceBase<TEntity, TKey> where TEntity : class, new()
    {
        Task<bool> ExistsAsync(TKey id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        #region Reading

        Task<TEntity> FindByIdAsync(TKey id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IAsyncEnumerable<TEntity> FindRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> ListAsync();

        #endregion

        #region Deleting

        Task<Result> DeleteByIdAsync(TKey id);
        Task<Result> DeleteAsync(TEntity user);
        Task<Result> DeleteAsync(Expression<Func<TEntity, bool>> expression);
        Task<Result> DeleteRangeAsync(IEnumerable<TEntity> user);

        #endregion

        #region Updating

        Task<Result> UpdateAsync(TEntity entity);
        Task<Result> UpdateRange(IEnumerable<TEntity> entites);

        #endregion
    }
}
