using FluentAssertions;
using StoreAPI;
using StoreAPI.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace StoreAPI_Tests_Integration.Services;

public class OrderServiceIntegration(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateAsync_EnsureReturnSuccessWhenOrderIsCreate()
    {
        var createOrderDto = new CreateOrderDTO
        {
            ClientId = 1,
            ProductList =
            [
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 2, Quantity = 1 },
                new() { ProductId = 3, Quantity = 1 }
            ],
            PaymentData = new()
            {
                CPF = "12345678911",
                Number = "1234567891234567",
                Expiration = DateTimeOffset.Now.AddYears(5),
                CVV = "123",
                Installments = 1
            }
        };

        var response = await _client.PostAsJsonAsync("api/Order/Create", createOrderDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}