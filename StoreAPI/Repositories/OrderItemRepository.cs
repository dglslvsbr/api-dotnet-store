using StoreAPI.AppContext;
using StoreAPI.Entities.Models;

namespace StoreAPI.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}