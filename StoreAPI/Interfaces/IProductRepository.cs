using StoreAPI.Entities.Models;

namespace StoreAPI.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetPaginatedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize);
}