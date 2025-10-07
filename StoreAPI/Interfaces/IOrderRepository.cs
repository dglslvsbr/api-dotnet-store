using StoreAPI.Entities.Models;

namespace StoreAPI.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllOrdersPaginatedAsync(int pageNumber, int pageSize);
}