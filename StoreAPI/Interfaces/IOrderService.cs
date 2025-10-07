using StoreAPI.DTOs;

namespace StoreAPI.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<ShowOrderDTO>> GetAllOrdersPaginatedAsync(int pageNumber, int pageSize);
    Task<ShowOrderDTO> GetAsync(int id);
    Task CreateAsync(int clientId, int installments, List<int> productsId);
    Task UpdateAsync(UpdateOrderDTO updateOrderDTO);
    Task DeleteAsync(ShowOrderDTO showOrderDTO);
}