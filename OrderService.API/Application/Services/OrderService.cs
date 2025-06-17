using OrderService.API.Application.DTOs;
using OrderService.API.Application.Interfaces;
using OrderService.API.Domain;
using OrderService.API.Infrastructure;


namespace OrderService.API.Application.Services;

public class OrderService(AppDbContext context, IProductClient productClient) : IOrderService
{
    public Order CreateOrder(CreateOrderRequest request)
    {
        var product = productClient.GetProductByIdAsync(request.ProductId).Result;

        if (product is null) throw new Exception($"Product with ID {request.ProductId} does not exist.");
        if (product.Quantity < request.Quantity) throw new Exception($"Only {product.Quantity} items available in stock");

        var order = new Order
        {
            UserId = request.UserId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            OrderedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        context.Orders.Add(order);
        context.SaveChanges();

        productClient.DecreaseStockAsync(request.ProductId, request.Quantity).Wait();

        return order;
    }

    public List<Order> GetAllOrders()
    {
        return context.Orders.ToList();
    }

    public Order? GetOrderById(int id)
    {
        return context.Orders.FirstOrDefault(order => order.Id == id);
    }

    public bool DeleteOrder(int id)
    {
        var order = context.Orders.FirstOrDefault(order => order.Id == id);
        if (order == null) return false;

        context.Orders.Remove(order);
        context.SaveChanges();
        return true;
    }
}