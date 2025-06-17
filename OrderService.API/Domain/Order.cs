using System.ComponentModel.DataAnnotations;

namespace OrderService.API.Domain;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public required string OrderedAt { get; set; }
}
