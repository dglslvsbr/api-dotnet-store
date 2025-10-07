using StoreAPI.DTOs;

namespace StoreAPI.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ShowProductDTO>> GetPaginatedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    Task<IEnumerable<ShowProductDTO>> GetAllPaginatedProductsAsync(int pageNumber, int pageSize);
    Task<ShowProductDTO> GetAsync(int id);
    Task CreateAsync(CreateProductDTO entity);
    Task UpdateAsync(UpdateProductDTO entity);
    Task DeleteAsync(ShowProductDTO entity);
}