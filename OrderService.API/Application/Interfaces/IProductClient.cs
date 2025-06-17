using OrderService.API.Application.DTOs;

namespace OrderService.API.Application.Interfaces;

public interface IProductClient
{
    Task<Product?> GetProductByIdAsync(int productId);
    Task DecreaseStockAsync(int productId, int quantity);
}
