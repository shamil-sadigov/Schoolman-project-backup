using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceBase<TEntity, TKey> where TEntity : class, new()
    {
        #region Reading

        Task<TEntity> FindAsync(TKey id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IAsyncEnumerable<TEntity> FindRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> Predicate);

        #endregion

        #region Deleting

        Task<Result> DeleteAsync(TKey id);
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
