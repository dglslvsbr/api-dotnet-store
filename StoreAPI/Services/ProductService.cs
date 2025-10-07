using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;

namespace StoreAPI.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : IProductService
{
    private static string ProductListKey => "CacheProduct";
    private static string ProductByIdKey(int id) => $"{ProductListKey}/{id}";

    public async Task<IEnumerable<ShowProductDTO>> GetPaginatedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
    {
        var productList = await unitOfWork.ProductRepository.GetPaginatedProductsByCategoryAsync(categoryId, pageNumber, pageSize);
        return mapper.Map<IEnumerable<ShowProductDTO>>(productList);
    }

    public async Task<IEnumerable<ShowProductDTO>> GetAllPaginatedProductsAsync(int pageNumber, int pageSize)
    {
        var productList = await memoryCache.GetOrSetAsync(ProductListKey, () => unitOfWork.ProductRepository.GetAllAsync());
        return mapper.Map<IEnumerable<ShowProductDTO>>(productList);
    }

    public async Task<ShowProductDTO> GetAsync(int id)
    {
        var product = await memoryCache.GetOrSetAsync(ProductByIdKey(id), () => unitOfWork.ProductRepository.GetAsync(id));
        return mapper.Map<ShowProductDTO>(product);
    }

    public async Task CreateAsync(CreateProductDTO entity)
    {
        await unitOfWork.ProductRepository.CreateAsync(mapper.Map<Product>(entity));
        await unitOfWork.CommitAsync();

        memoryCache.Remove(ProductListKey);
    }

    public async Task UpdateAsync(UpdateProductDTO entity)
    {
        unitOfWork.ProductRepository.Update(mapper.Map<Product>(entity));
        await unitOfWork.CommitAsync();

        RemoveAllCache(entity.Id);
    }

    public async Task DeleteAsync(ShowProductDTO entity)
    {
        unitOfWork.ProductRepository.Delete(mapper.Map<Product>(entity));
        await unitOfWork.CommitAsync();

        RemoveAllCache(entity.Id);
    }

    private void RemoveAllCache(int id)
    {
        memoryCache.Remove(ProductListKey);
        memoryCache.Remove(ProductByIdKey(id));
    }
}