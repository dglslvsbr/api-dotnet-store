using StoreAPI.DTOs;

namespace StoreAPI.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<ShowCategoryDTO>> GetAllCategoriesPaginatedAsync(int pageNumber, int pageSize);
    Task<ShowCategoryDTO> GetCategoryWithProductsAsync(int id);
    Task<ShowCategoryDTO> GetAsync(int id);
    Task CreateAsync(CreateCategoryDTO categoryDto);
    Task UpdateAsync(UpdateCategoryDTO updateCategoryDto);
    Task DeleteAsync(ShowCategoryDTO categoryDto);
}