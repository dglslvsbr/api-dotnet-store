using StoreAPI.AppContext;
using StoreAPI.Entities.Models;

namespace StoreAPI.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}