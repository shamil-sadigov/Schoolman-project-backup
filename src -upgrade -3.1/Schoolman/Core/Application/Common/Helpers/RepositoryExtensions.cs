using Application.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Helpers
{
    public static class RepositoryExtensions
    {

        /// <summary>
        /// Simple method for merging addition and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> AddAndSaveAsync<T>(this IRepository<T> repo, T entity) where T : class
        {
            await repo.Set.AddAsync(entity);
            return await repo.SaveChangesAsync() > 0 ? true : false;
        }


        public static async Task<bool> UpdateAndSaveAsync<T>(this IRepository<T> repo, T entity) where T : class
        {
             repo.Set.Update(entity);
            return await repo.SaveChangesAsync() > 0 ? true : false;
        }
        public static async Task<bool> UpdateRangeAndSaveAsync<T>(this IRepository<T> repo, IEnumerable<T> entites) where T : class
        {
            repo.Set.UpdateRange(entites);
            return await repo.SaveChangesAsync() > 0 ? true : false;
        }


        /// <summary>
        /// Simple method for merthing removing and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveAndSaveAsync<T>(this IRepository<T> repo, T entity) where T : class
        {
            repo.Set.Remove(entity);
            return await repo.SaveChangesAsync() > 0 ? true : false;
        }

        public static async Task<bool> RemoveRangeAndSaveAsync<T>(this IRepository<T> repo, IEnumerable<T> entites) where T : class
        {
            repo.Set.RemoveRange(entites);
            return await repo.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
