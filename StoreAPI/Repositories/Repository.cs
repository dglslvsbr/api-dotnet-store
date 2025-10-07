using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    public readonly AppDbContext _context = context;

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

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}