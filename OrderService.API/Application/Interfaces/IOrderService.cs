using OrderService.API.Application.DTOs;
using OrderService.API.Domain;

namespace OrderService.API.Application.Interfaces;

public interface IOrderService
{
    Order CreateOrder(CreateOrderRequest request);
    List<Order> GetAllOrders();
    Order? GetOrderById(int id);
    bool DeleteOrder(int id);
}