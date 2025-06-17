using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderService.API.Application.DTOs;
using OrderService.API.Application.Interfaces;
using OrderService.API.Domain;
using OrderService.API.Infrastructure;
using Xunit;

namespace OrderService.API.Tests.Application.Services;

public class OrderServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: new Random().Next().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void CreateOrder_Should_Create_Valid_Order_And_Decrease_Stock()
    {
        var dbContext = CreateDbContext();
        var productClientMock = new Mock<IProductClient>();

        productClientMock.Setup(x => x.GetProductByIdAsync(100))
            .ReturnsAsync(new Product { Id = 100, Quantity = 5 });

        productClientMock.Setup(x => x.DecreaseStockAsync(100, 2))
            .Returns(Task.CompletedTask);

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, productClientMock.Object);

        var request = new CreateOrderRequest { UserId = 1, ProductId = 100, Quantity = 2 };
        var order = orderService.CreateOrder(request);

        Assert.NotNull(order);
        Assert.Equal(100, order.ProductId);
        Assert.Equal(2, order.Quantity);

        var saved = dbContext.Orders.FirstOrDefault();
        Assert.NotNull(saved);
    }

    [Fact]
    public void CreateOrder_Should_Throw_Exception_If_Product_Not_Found()
    {
        var dbContext = CreateDbContext();
        var productClientMock = new Mock<IProductClient>();

        productClientMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, productClientMock.Object);

        var request = new CreateOrderRequest { UserId = 1, ProductId = 999, Quantity = 1 };

        var ex = Assert.Throws<Exception>(() => orderService.CreateOrder(request));
        Assert.Contains("does not exist", ex.Message);
    }

    [Fact]
    public void CreateOrder_Should_Throw_Exception_If_Insufficient_Stock()
    {
        var dbContext = CreateDbContext();
        var productClientMock = new Mock<IProductClient>();

        productClientMock.Setup(x => x.GetProductByIdAsync(100))
            .ReturnsAsync(new Product { Id = 100, Quantity = 1 });

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, productClientMock.Object);

        var request = new CreateOrderRequest { UserId = 1, ProductId = 100, Quantity = 5 };

        var ex = Assert.Throws<Exception>(() => orderService.CreateOrder(request));
        Assert.Contains("Only 1 items available", ex.Message);
    }

    [Fact]
    public void GetAllOrders_Should_Return_All_Orders()
    {
        var dbContext = CreateDbContext();
        dbContext.Orders.Add(new Order { ProductId = 1, UserId = 1, Quantity = 2, OrderedAt = "2024-01-01 10:00" });
        dbContext.Orders.Add(new Order { ProductId = 2, UserId = 1, Quantity = 1, OrderedAt = "2024-01-02 11:00" });
        dbContext.SaveChanges();
        
        var productClientMock = new Mock<IProductClient>();
        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, productClientMock.Object);
    
        var result = orderService.GetAllOrders();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetOrderById_Should_Return_Order_If_Exists()
    {
        var dbContext = CreateDbContext();
        var order = new Order { ProductId = 1, UserId = 2, Quantity = 3, OrderedAt = "2024-01-01 10:00" };
        dbContext.Orders.Add(order);
        dbContext.SaveChanges();

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, new Mock<IProductClient>().Object);

        var result = orderService.GetOrderById(order.Id);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result?.Id);
    }

    [Fact]
    public void GetOrderById_Should_Return_Null_If_Not_Found()
    {
        var dbContext = CreateDbContext();

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, new Mock<IProductClient>().Object);

        var result = orderService.GetOrderById(999);

        Assert.Null(result);
    }

    [Fact]
    public void DeleteOrder_Should_Remove_Order_If_Exists()
    {
        var dbContext = CreateDbContext();
        var order = new Order { ProductId = 3, UserId = 2, Quantity = 1, OrderedAt = "2024-01-03 12:00" };
        dbContext.Orders.Add(order);
        dbContext.SaveChanges();

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, new Mock<IProductClient>().Object);

        var result = orderService.DeleteOrder(order.Id);

        Assert.True(result);
        Assert.Null(dbContext.Orders.FirstOrDefault(o => o.Id == order.Id));
    }

    [Fact]
    public void DeleteOrder_Should_Return_False_If_Not_Exists()
    {
        var dbContext = CreateDbContext();

        var orderService = new OrderService.API.Application.Services.OrderService(dbContext, new Mock<IProductClient>().Object);

        var result = orderService.DeleteOrder(999);

        Assert.False(result);
    }
}