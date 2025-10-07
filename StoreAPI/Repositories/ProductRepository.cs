using StoreAPI.Context;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Services;

namespace StoreAPI.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IEnumerable<Product>> GetPaginatedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
    {
        var source = _context.Product.Where(x => x.CategoryId == categoryId).OrderBy(x => x.Id).AsQueryable();
        return await PaginatedService.EntityPaginated(source, pageNumber, pageSize);
    }

    public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
    {
        var source = _context.Product.OrderBy(x => x.Id).AsQueryable();
        return await PaginatedService.EntityPaginated(source, pageNumber, pageSize);
    }
}