using StoreAPI.AppContext;
using StoreAPI.Entities.Models;

namespace StoreAPI.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}