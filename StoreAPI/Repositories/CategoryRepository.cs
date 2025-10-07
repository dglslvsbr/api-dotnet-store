using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Services;

namespace StoreAPI.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllCategoriesPaginatedAsync(int pageSize, int pageNumber)
    {
        var source = _context.Category.Include(x => x.Product).OrderBy(x => x.Id).AsQueryable();
        return await PaginatedService.EntityPaginated(source, pageNumber, pageSize);
    }

    public async Task<Category> GetCategoryWithProductsAsync(int id)
    {
        return (await _context.Category.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id))!;
    }
}