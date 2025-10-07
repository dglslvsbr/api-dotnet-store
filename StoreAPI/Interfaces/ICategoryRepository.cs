using StoreAPI.Entities.Models;

namespace StoreAPI.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesPaginatedAsync(int pageNumber, int pageSize);
    Task<Category> GetCategoryWithProductsAsync(int id);
}