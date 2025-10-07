using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;

namespace StoreAPI.Services;

public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : ICategoryService
{
    private static string CategoryListKey => "CategoryCache";
    private static string CategoryByIdKey(int id) => $"{CategoryListKey}/{id}";

    public async Task<IEnumerable<ShowCategoryDTO>> GetAllCategoriesPaginatedAsync(int pageNumber, int pageSize)
    {
        var categoriesList = await memoryCache.GetOrSetAsync(CategoryListKey, 
            () => unitOfWork.CategoryRepository.GetAllCategoriesPaginatedAsync(pageNumber, pageSize));
        return mapper.Map<IEnumerable<ShowCategoryDTO>>(categoriesList);
    }

    public async Task<ShowCategoryDTO> GetCategoryWithProductsAsync(int id)
    {
        var categoryWithProducts = await memoryCache.GetOrSetAsync(CategoryByIdKey(id), () => unitOfWork.CategoryRepository.GetCategoryWithProductsAsync(id));
        return mapper.Map<ShowCategoryDTO>(categoryWithProducts);
    }

    public async Task<ShowCategoryDTO> GetAsync(int id)
    {
        var category = await memoryCache.GetOrSetAsync(CategoryByIdKey(id), () => unitOfWork.CategoryRepository.GetAsync(id));
        return mapper.Map<ShowCategoryDTO>(category);
    }

    public async Task CreateAsync(CreateCategoryDTO categoryDto)
    {
        await unitOfWork.CategoryRepository.CreateAsync(mapper.Map<Category>(categoryDto));
        await unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(UpdateCategoryDTO updateCategoryDto)
    {
        unitOfWork.CategoryRepository.Update(mapper.Map<Category>(updateCategoryDto));
        await unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(ShowCategoryDTO categoryDto)
    {
        unitOfWork.CategoryRepository.Delete(mapper.Map<Category>(categoryDto));
        await unitOfWork.CommitAsync();
    }
}