using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceBase<Type>
    {
        #region Reading

        Task<Type> FindAsync(string id);
        Task<Type> FindAsync(Expression<Func<Type, bool>> predicate);
        IAsyncEnumerable<Type> FindRangeAsync(Expression<Func<Type, bool>> predicate);

        Task<bool> ExistAsync(Expression<Func<Type, bool>> Predicate);

        #endregion

        #region Deleting

        Task<Result> DeleteAsync(string userId);
        Task<Result> DeleteAsync(Type user);
        Task<Result> DeleteAsync(Expression<Func<Type, bool>> expression);
        Task<Result> DeleteRangeAsync(IEnumerable<Type> user);

        #endregion

        #region Updating

        Task<Result> UpdateAsync(Type entity);
        Task<Result> UpdateRange(IEnumerable<Type> entites);

        #endregion
    }
}
