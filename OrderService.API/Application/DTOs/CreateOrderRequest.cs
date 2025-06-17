namespace OrderService.API.Application.DTOs;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
