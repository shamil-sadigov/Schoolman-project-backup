using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Repository that enables you to interact with Database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T: class
    {
        DbSet<T> Set { get; }
        Task<int> SaveChangesAsync();
        DbContext Context { get; }
    }
}