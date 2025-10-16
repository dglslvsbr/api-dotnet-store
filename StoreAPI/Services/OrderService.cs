using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

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

    public async Task<ServiceResult> CreateAsync(CreateOrderDTO createOrderDto)
    {
        try
        {
            if (createOrderDto is null)
                return ServiceResult.Fail("Invalid data");

            if (createOrderDto.ProductList.Count == 0)
                return ServiceResult.Fail("No product selected");

            List<OrderItem> orderItems = [];

            foreach (BuyProductDTO product in createOrderDto.ProductList)
            {
                var currentProduct = await unitOfWork.ProductRepository.GetAsync(product.ProductId);

                if (currentProduct is null)
                    return ServiceResult.Fail("Product not found in the database");

                orderItems.Add(new OrderItem
                {
                    ProductId = currentProduct.Id,
                    UnitPrice = currentProduct.Price,
                    Quantity = product.Quantity
                });
            }

            var order = new Order
            {
                CreatAt = DateTimeOffset.Now,
                CurrentState = OrderState.Processing,
                Installments = createOrderDto.PaymentData.Installments,
                ClientId = createOrderDto.ClientId,
                OrderItem = orderItems
            };

            var client = await unitOfWork.ClientRepository.GetByCPFAsync(createOrderDto.PaymentData.CPF!);

            if (client is null)
                return ServiceResult.Fail("Client not found in the database");

            if (client.CreditCard is null ||
                client.CPF != createOrderDto.PaymentData.CPF ||
                client.CreditCard.Number != createOrderDto.PaymentData.Number ||
                client.CreditCard.Expiration.Year != createOrderDto.PaymentData.Expiration.Year ||
                client.CreditCard.CVV != createOrderDto.PaymentData.CVV)
                return ServiceResult.Fail("Invalid payment data");

            if (order.OrderTotal > client.CreditCard.MaxLimit ||
                order.OrderTotal > client.CreditCard.RemainingLimit)
                return ServiceResult.Fail("Insufficient card limit");

            await unitOfWork.BeginTransaction();

            client.CreditCard.UsedLimit += order.OrderTotal;

            unitOfWork.ClientRepository.Update(client);

            await unitOfWork.OrderRepository.CreateAsync(order);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();

            memoryCache.Remove(OrderListKey);
            memoryCache.Remove(ClientService.ClientListKey);
            memoryCache.Remove(ClientService.ClientByIdKey(createOrderDto.ClientId));

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            Console.WriteLine("An error occurred: " + ex.Message);
            return new ServiceResult { Success = false, ErrorMessage = "An error occurred while processed the order", Exception = ex };
        }
    }

    public async Task UpdateAsync(UpdateOrderDTO updateOrderDTO)
    {
        unitOfWork.OrderRepository.Update(mapper.Map<Order>(updateOrderDTO));
        await unitOfWork.SaveChangesAsync();

        RemoveAllCache(updateOrderDTO.Id);
    }

    public async Task DeleteAsync(ShowOrderDTO showOrderDTO)
    {
        unitOfWork.OrderRepository.Delete(mapper.Map<Order>(showOrderDTO));
        await unitOfWork.SaveChangesAsync();

        RemoveAllCache(showOrderDTO.Id);
    }

    private void RemoveAllCache(int id)
    {
        memoryCache.Remove(OrderListKey);
        memoryCache.Remove(OrderByIdKey(id));
    }
}