using Microsoft.EntityFrameworkCore;
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