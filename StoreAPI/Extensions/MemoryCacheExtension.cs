using Microsoft.Extensions.Caching.Memory;

namespace StoreAPI.Extensions;

public static class MemoryCacheExtension
{
    public static async Task<T> GetOrSetAsync<T>(this IMemoryCache memoryCache, string key, Func<Task<T>> expression)
    {
        if (!memoryCache.TryGetValue(key, out T? result))
        {
            result = await expression();
            memoryCache.Set(key, result, DefaultCacheOptions);
            Console.WriteLine("The data was not retrieved from the cache");
            return result!;
        }
        Console.WriteLine("The data was retrieved from the cache");
        return result!;
    }

    private static MemoryCacheEntryOptions DefaultCacheOptions => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };
}