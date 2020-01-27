using Application.Services;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly DataContext dataContext;

        public Repository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public DbSet<T> Set()
        {
            return dataContext.Set<T>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

       
    }
}
