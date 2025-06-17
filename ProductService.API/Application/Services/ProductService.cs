using ProductService.API.Application.DTO;
using ProductService.API.Application.Interfaces;
using ProductService.API.Infrastructure;
using UserService.API.Domain;

namespace ProductService.API.Application.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public Product CreateProduct(CreateProductRequest req)
    {
        var product = new Product
        {
            Name = req.Name,
            Price = req.Price,
            Quantity = req.Quantity
        };

        context.Products.Add(product);
        context.SaveChanges();

        return product;
    }

    public List<Product> GetAllProducts()
    {
        return context.Products.ToList();
    }

    public Product? GetProductById(int id)
    {
        return context.Products.Find(id);
    }

    public Product? UpdateProduct(UpdateProductRequest req)
    {
        Console.WriteLine(req.Id);

        var product = context.Products.Find(req.Id);
        if (product == null) return null;
        product.Name = req.Name ?? product.Name;
        product.Price = req.Price ?? product.Price;
        product.Quantity = req.Quantity ?? product.Quantity;

        context.SaveChanges();

        return product;
    }

    public bool DeleteProduct(int id)
    {
        var product = context.Products.Find(id);
        if (product == null) return false;

        context.Products.Remove(product);
        context.SaveChanges();

        return true;
    }

    public void ReduceStock(int id, int quantity)
    {
        var product = context.Products.Find(id);

        if (product == null)
            throw new InvalidOperationException("Product not found");

        if (product.Quantity < quantity)
            throw new InvalidOperationException("Insufficient stock");

        product.Quantity -= quantity;
        context.SaveChanges();
    }
}