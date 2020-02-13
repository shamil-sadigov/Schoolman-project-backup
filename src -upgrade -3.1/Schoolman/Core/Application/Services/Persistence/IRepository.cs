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
        IEnumerable<TEntity> FindArrange(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(params TEntity[] entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> AsNoTracking();
        void Remove(TEntity entity);
        void RemoveRange(params TEntity[] entities);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


        Task<int> SaveChangesAsync();
        DbContext Context { get; }
    }
}