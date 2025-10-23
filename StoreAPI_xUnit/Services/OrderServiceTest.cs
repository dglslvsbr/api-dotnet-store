using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Services;

namespace StoreAPI_Tests_Unit.Services;

public class OrderServiceTest
{
    private static CreateOrderDTO CreateOrderDto()
    {
        return new CreateOrderDTO
        {
            ClientId = 1,
            ProductList = [
                new() { ProductId = 1, Quantity = 1},
                new() { ProductId = 2, Quantity = 1 },
                new() { ProductId = 3, Quantity = 1 }
            ],
            PaymentData = new()
            {
                CPF = "123",
                Number = "1234",
                CVV = "123",
                Expiration = new DateTimeOffset(2030, 12, 01, 0, 0, 0, TimeSpan.Zero),
                Installments = 1
            }
        };
    }

    private static List<Product> ProductDatabaseFake()
    {
        return
        [
            new() { Id = 1, Price = 100m },
            new() { Id = 2, Price = 200m },
            new() { Id = 3, Price = 300m }
        ];
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenOrderIsCreate()
    {
        var createOrderDto = CreateOrderDto();
        var productDatabaseFake = ProductDatabaseFake();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(x => x.ProductRepository.GetAsync(It.IsAny<int>()))!
            .ReturnsAsync((int id) => productDatabaseFake.FirstOrDefault(x => x.Id == id));

        unitOfWorkMock.Setup(x => x.ClientRepository.GetByCPFAsync(It.IsAny<string>()))
            .ReturnsAsync(new Client()
            {
                CPF = "123",
                CreditCard = new()
                {
                    Number = createOrderDto.PaymentData.Number,
                    Expiration = createOrderDto.PaymentData.Expiration,
                    CVV = createOrderDto.PaymentData.CVV,
                    UsedLimit = 1000m,
                    MaxLimit = 5000m
                }
            });

        unitOfWorkMock.Setup(x => x.ClientRepository.Update(It.IsAny<Client>()));

        unitOfWorkMock.Setup(x =>
        x.OrderRepository.CreateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var orderService = new OrderService(unitOfWorkMock.Object,
            null!, new Mock<IMemoryCache>().Object);

        var result = await orderService.CreateAsync(createOrderDto);

        result.Success.Should().BeTrue();

        unitOfWorkMock.Verify(x =>
        x.OrderRepository.CreateAsync(It.Is<Order>(x =>
        x.ClientId == createOrderDto.ClientId &&
        x.OrderItem!.Count == createOrderDto.ProductList.Count &&
        x.OrderTotal == productDatabaseFake.Sum(x => x.Price))), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenOrderIsNull()
    {
        var orderService = new OrderService(new Mock<IUnitOfWork>().Object, null!, null!);

        var result = await orderService.CreateAsync(null!);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid data");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenProductListIsEmpty()
    {
        var createOrderDto = CreateOrderDto();
        createOrderDto.ProductList = [];

        var orderService = new OrderService(new Mock<IUnitOfWork>().Object, null!, null!);

        var result = await orderService.CreateAsync(createOrderDto);

        createOrderDto.ProductList.Should().BeEmpty();
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenProductNotFound()
    {
        var productDatabaseFake = new List<Product>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        var createOrderDto = CreateOrderDto();
        createOrderDto.ProductList = [new() { ProductId = 0 }];

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>()))!
            .ReturnsAsync((int id) => productDatabaseFake.FirstOrDefault(x => x.Id == id));

        var orderService = new OrderService(unitOfWorkMock.Object, null!, null!);

        var result = await orderService.CreateAsync(createOrderDto);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Product not found in the database");

        unitOfWorkMock.Verify(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenClientNotFound()
    {
        var createOrderDto = CreateOrderDto();
        createOrderDto.ProductList = [new()];
        createOrderDto.PaymentData.CPF = "000";

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(x => x.ProductRepository.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product());

        unitOfWorkMock.Setup(x =>
        x.ClientRepository.GetByCPFAsync(createOrderDto.PaymentData.CPF))
            .ReturnsAsync((Client)null!);

        var orderService = new OrderService(unitOfWorkMock.Object, null!, null!);

        var result = await orderService.CreateAsync(createOrderDto);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Client not found in the database");

        unitOfWorkMock.Verify(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>()), Times.AtLeastOnce);

        unitOfWorkMock.Verify(x =>
        x.ClientRepository.GetByCPFAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenPaymentIsInvalid()
    {
        var createOrderDto = CreateOrderDto();
        createOrderDto.ProductList = [new()];
        createOrderDto.PaymentData = new();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>())).ReturnsAsync(new Product());

        unitOfWorkMock.Setup(x =>
        x.ClientRepository.GetByCPFAsync(It.IsAny<string>())).ReturnsAsync(new Client());

        var orderService = new OrderService(unitOfWorkMock.Object, null!, null!);

        var result = await orderService.CreateAsync(createOrderDto);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid payment data");

        unitOfWorkMock.Verify(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>()), Times.AtLeastOnce);

        unitOfWorkMock.Verify(x =>
        x.ClientRepository.GetByCPFAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenLimitCreditCardIsInvalid()
    {
        var createOrderDto = CreateOrderDto();
        var productDatabaseFake = ProductDatabaseFake();
        productDatabaseFake[0].Price = 5000m;

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(x => x.ProductRepository.GetAsync(It.IsAny<int>()))!
            .ReturnsAsync((int id) => productDatabaseFake.FirstOrDefault(x => x.Id == id));

        unitOfWorkMock.Setup(x => x.ClientRepository.GetByCPFAsync(It.IsAny<string>()))
            .ReturnsAsync(new Client()
            {
                CPF = "123",
                CreditCard = new()
                {
                    Number = createOrderDto.PaymentData.Number,
                    Expiration = createOrderDto.PaymentData.Expiration,
                    CVV = createOrderDto.PaymentData.CVV,
                    UsedLimit = 1000m,
                    MaxLimit = 5000m
                }
            });

        var orderService = new OrderService(unitOfWorkMock.Object, null!, null!);

        var result = await orderService.CreateAsync(createOrderDto);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Insufficient card limit");

        unitOfWorkMock.Verify(x =>
        x.ProductRepository.GetAsync(It.IsAny<int>()), Times.AtLeastOnce);

        unitOfWorkMock.Verify(x =>
        x.ClientRepository.GetByCPFAsync(It.IsAny<string>()), Times.Once);
    }
}