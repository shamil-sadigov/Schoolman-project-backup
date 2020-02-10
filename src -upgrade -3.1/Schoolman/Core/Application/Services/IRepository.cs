using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IRepository<T> where T: class
    {
        DbSet<T> Set { get; }
        Task<int> SaveChangesAsync();
        DbContext Context { get; }
    }
}