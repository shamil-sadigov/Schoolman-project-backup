using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Helpers
{
    public static class DbContextExtensions
    {

        /// <summary>
        /// Simple method for merging addition and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<int> AddAndSaveAsync<T>(this DbContext db, T entity) where T : class
        {
            await db.Set<T>().AddAsync(entity);
            return await db.SaveChangesAsync();
        }



        /// <summary>
        /// Simple method for merthing removing and saving entity in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<int> RemoveAndSaveAsync<T>(this DbContext db, T entity) where T : class
        {
            db.Set<T>().Remove(entity);
            return await db.SaveChangesAsync();
        }

    }
}
