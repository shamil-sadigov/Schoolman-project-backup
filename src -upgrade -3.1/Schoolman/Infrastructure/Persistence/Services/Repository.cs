using Application.Services;
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
        private readonly SchoolmanContext context;
        public Repository(SchoolmanContext context)
        {
            this.context = context;
        }


        public DbSet<T> Collection
        {
            get => context.Set<T>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
