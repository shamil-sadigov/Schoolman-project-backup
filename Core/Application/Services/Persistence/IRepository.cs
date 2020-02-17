using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Repository that enables you to interact with Database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity: class
    {
        Task<TEntity> FindAsync(object key);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        public IQueryable<TEntity> FindRange(Expression<Func<TEntity, bool>> predicate);
        Task AddOrUpdateAsync(TEntity entity);
        Task AddRangeAsync(params TEntity[] entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> AsQueryable();
        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(params TEntity[] entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        DbContext Context { get; }
    }
}