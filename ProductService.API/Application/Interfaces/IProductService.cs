using ProductService.API.Application.DTO;
using UserService.API.Domain;

namespace ProductService.API.Application.Interfaces;

public interface IProductService
{
    Product CreateProduct(CreateProductRequest req);
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    Product? UpdateProduct(UpdateProductRequest req);
    bool DeleteProduct(int id);
    void ReduceStock(int id, int quantity);
}
