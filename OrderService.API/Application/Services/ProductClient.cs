using OrderService.API.Application.DTOs;
using OrderService.API.Application.Interfaces;

namespace OrderService.API.Application.Services;

public class ProductClient(HttpClient httpClient) : IProductClient
{

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var response = await httpClient.GetAsync($"/api/Products/{productId}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task DecreaseStockAsync(int id, int quantity)
    {
        var response = await httpClient.PatchAsync($"/api/Products/{id}/reduce-stock/{quantity}", null);
        response.EnsureSuccessStatusCode();
    }
}
