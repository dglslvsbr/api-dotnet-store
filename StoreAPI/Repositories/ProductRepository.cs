using StoreAPI.AppContext;
using StoreAPI.Entities.Models;

namespace StoreAPI.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}