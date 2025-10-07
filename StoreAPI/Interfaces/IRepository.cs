namespace StoreAPI.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(int id);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}