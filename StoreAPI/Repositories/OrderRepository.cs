using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Services;

namespace StoreAPI.Repositories;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllOrdersPaginatedAsync(int pageNumber, int pageSize)
    {
        var source = _context.Order.Include(x => x.OrderItem!).ThenInclude(x => x.Product).OrderBy(x => x.Id).AsQueryable();
        return await PaginatedService.EntityPaginated(source, pageNumber, pageSize);
    }
}