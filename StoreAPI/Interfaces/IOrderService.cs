using StoreAPI.DTOs;
using StoreAPI.Useful;

namespace StoreAPI.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<ShowOrderDTO>> GetAllOrdersPaginatedAsync(int pageNumber, int pageSize);
    Task<ShowOrderDTO> GetAsync(int id);
    Task<ServiceResult> CreateAsync(CreateOrderDTO createOrderDto);
    Task UpdateAsync(UpdateOrderDTO updateOrderDTO);
    Task DeleteAsync(ShowOrderDTO showOrderDTO);
}