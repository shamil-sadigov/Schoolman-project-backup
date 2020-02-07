using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class BaseRepository<T> : IRepository<T> where T: class
    {
        private readonly SchoolmanContext context;
        public BaseRepository(SchoolmanContext context)
        {
            this.context = context;
        }


        public DbSet<T> Set
        {
            get => context.Set<T>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public DbContext Context { get => context; }
    }
}
