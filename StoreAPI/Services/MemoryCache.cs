using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.AppContext;
using StoreAPI.Interfaces;

namespace StoreAPI.Services
{
    public class MemoryCache<T> : ISystemCache<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public MemoryCache(AppDbContext context, IMemoryCache memoryCache, ILogger<MemoryCache> logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> TryGetCacheList(string key)
        {
            if(!_memoryCache.TryGetValue(key, out IEnumerable<T>? entities))
            {
                entities = await _context.Set<T>().ToListAsync();

                SetCacheList(key, entities);

                _logger.LogInformation("(MemoryCache): The data was fetched from the database.");
                return entities;
            }
            _logger.LogInformation("(MemoryCache): The data was fetched from the cache.");
            return entities!;
        }

        public async Task<T> TryGetCacheUnique(string key, int id)
        {
            if (!_memoryCache.TryGetValue(key, out T? entity))
            {
                entity = await _context.Set<T>().FindAsync(id);

                SetCacheUnique(key, entity!);

                _logger.LogInformation("(MemoryCache): The data was fetched from the database.");
                return entity!;
            }
            _logger.LogInformation("(MemoryCache): The data was fetched from the cache.");
            return entity!;
        }

        public void SetCacheList(string key, IEnumerable<T> entities)
        {
            _memoryCache.Set(key, entities);
            _logger.LogInformation("(MemoryCache): A list with data has been added to the memory cache.");
        }

        public void SetCacheUnique(string key, T entity)
        {
            _memoryCache.Set(key, entity);
            _logger.LogInformation("(MemoryCache): A data has been added to the memory cache");
        }

        public void InvalidCache(string key)
        {
            _memoryCache.Remove(key);
            _logger.LogInformation("(MemoryCache): A cache has been removed from memory");
        }
    }
}