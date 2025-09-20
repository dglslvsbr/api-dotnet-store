using Microsoft.EntityFrameworkCore;
using StoreAPI.AppContext;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return (await _context.Set<T>().FindAsync(id))!;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}