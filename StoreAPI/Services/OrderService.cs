using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;

namespace StoreAPI.Services;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : IOrderService
{
    public IUnitOfWork UnitOfWork { get; private set; } = unitOfWork ?? throw new ArgumentException("Unit Of Work is null");

    private static string OrderListKey => "CacheOrder";
    private static string OrderByIdKey(int id) => $"{OrderListKey}/{id}";
    
    public async Task<IEnumerable<ShowOrderDTO>> GetAllOrdersPaginatedAsync(int pageNumber, int pageSize)
    {
        var orderList = await memoryCache.GetOrSetAsync(OrderListKey, () => unitOfWork.OrderRepository.GetAllOrdersPaginatedAsync(pageNumber, pageSize));
        return mapper.Map<IEnumerable<ShowOrderDTO>>(orderList);
    }

    public async Task<ShowOrderDTO> GetAsync(int id)
    {
        var order = await memoryCache.GetOrSetAsync(OrderByIdKey(id), () => unitOfWork.OrderRepository.GetAsync(id));
        return mapper.Map<ShowOrderDTO>(order);
    }

    public async Task CreateAsync(int clientId, int installments, List<int> productsId)
    {
        List<Product> allProducts = [];
        foreach (int n in productsId)
        {
            var currentProduct = await unitOfWork.ProductRepository.GetAsync(n)
                ?? throw new ArgumentException("The Product not found");
            allProducts.Add(currentProduct);
        }

        var orderItems = allProducts.Select(x => new OrderItem
        {
            ProductId = x.Id,
            UnitPrice = x.Price,
            Quantity = x.Quantity
        }).ToList();

        var order = new Order
        {
            CreatAt = DateTimeOffset.Now,
            CurrentState = OrderState.Processing,
            Installments = installments,
            ClientId = clientId,
            OrderItem = orderItems
        };

        await unitOfWork.OrderRepository.CreateAsync(order);
        await unitOfWork.CommitAsync();

        memoryCache.Remove(OrderListKey);
    }

    public async Task UpdateAsync(UpdateOrderDTO updateOrderDTO)
    {
        unitOfWork.OrderRepository.Update(mapper.Map<Order>(updateOrderDTO));
        await unitOfWork.CommitAsync();

        RemoveAllCache(updateOrderDTO.Id);
    }

    public async Task DeleteAsync(ShowOrderDTO showOrderDTO)
    {
        unitOfWork.OrderRepository.Delete(mapper.Map<Order>(showOrderDTO));
        await unitOfWork.CommitAsync();

        RemoveAllCache(showOrderDTO.Id);
    }

    private void RemoveAllCache(int id)
    {
        memoryCache.Remove(OrderListKey);
        memoryCache.Remove(OrderByIdKey(id));
    }
}