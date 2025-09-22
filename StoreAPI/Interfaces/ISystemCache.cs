namespace StoreAPI.Interfaces
{
    public interface ISystemCache<T>
    {
        Task<IEnumerable<T>> TryGetCacheList(string key);
        Task<T> TryGetCacheUnique(string key, int id);
        void SetCacheList(string key, IEnumerable<T> entities);
        void SetCacheUnique(string key, T entity);
        void InvalidCache(string key);
    }
}