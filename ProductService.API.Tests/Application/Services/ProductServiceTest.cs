using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Application.DTO;
using ProductService.API.Infrastructure;
using UserService.API.Domain;
using Xunit;

namespace ProductService.API.Tests.Application.Services;

public class ProductServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateProduct_ShouldAddNewProduct()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        var req = new CreateProductRequest { Name = "Mouse", Price = 199, Quantity = 10 };
        var result = service.CreateProduct(req);

        Assert.NotNull(result);
        Assert.Equal("Mouse", result.Name);
        Assert.Equal(199, result.Price);
        Assert.Equal(10, result.Quantity);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public void GetAllProducts_ShouldReturnAll()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        context.Products.Add(new Product { Name = "A", Price = 100, Quantity = 1 });
        context.Products.Add(new Product { Name = "B", Price = 200, Quantity = 2 });
        context.SaveChanges();

        var result = service.GetAllProducts().ToList();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetProductById_ShouldReturnCorrectProduct()
    {
        var context = GetDbContext();
        var product = new Product { Name = "Keyboard", Price = 300, Quantity = 5 };
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductService.API.Application.Services.ProductService(context);
        var result = service.GetProductById(product.Id);

        Assert.NotNull(result);
        Assert.Equal("Keyboard", result.Name);
    }

    [Fact]
    public void GetProductById_ShouldReturnNull_IfNotFound()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        var result = service.GetProductById(999);
        Assert.Null(result);
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateFields()
    {
        var context = GetDbContext();
        var product = new Product { Name = "Old", Price = 500, Quantity = 20 };
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductService.API.Application.Services.ProductService(context);
        var req = new UpdateProductRequest { Id = product.Id, Name = "New", Price = 550, Quantity = 25 };
        var result = service.UpdateProduct(req);

        Assert.NotNull(result);
        Assert.Equal("New", result!.Name);
        Assert.Equal(550, result.Price);
        Assert.Equal(25, result.Quantity);
    }

    [Fact]
    public void UpdateProduct_ShouldReturnNull_IfIdNotExist()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        var req = new UpdateProductRequest { Id = 123, Name = "NotExist" };
        var result = service.UpdateProduct(req);

        Assert.Null(result);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProduct()
    {
        var context = GetDbContext();
        var product = new Product { Name = "ToDelete", Price = 150, Quantity = 2 };
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductService.API.Application.Services.ProductService(context);
        var deleted = service.DeleteProduct(product.Id);

        Assert.True(deleted);
        Assert.Null(context.Products.Find(product.Id));
    }

    [Fact]
    public void DeleteProduct_ShouldReturnFalse_IfNotExist()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        var deleted = service.DeleteProduct(12345);
        Assert.False(deleted);
    }

    [Fact]
    public void ReduceStock_ShouldDecreaseQuantity()
    {
        var context = GetDbContext();
        var product = new Product { Name = "Item", Price = 200, Quantity = 10 };
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductService.API.Application.Services.ProductService(context);
        service.ReduceStock(product.Id, 3);

        var updated = context.Products.Find(product.Id);
        Assert.Equal(7, updated!.Quantity);
    }

    [Fact]
    public void ReduceStock_ShouldThrow_IfNotEnoughQuantity()
    {
        var context = GetDbContext();
        var product = new Product { Name = "Limited", Price = 99, Quantity = 2 };
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductService.API.Application.Services.ProductService(context);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            service.ReduceStock(product.Id, 5)
        );

        Assert.Equal("Insufficient stock", ex.Message);
    }

    [Fact]
    public void ReduceStock_ShouldThrow_IfProductNotFound()
    {
        var context = GetDbContext();
        var service = new ProductService.API.Application.Services.ProductService(context);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            service.ReduceStock(999, 1)
        );

        Assert.Equal("Product not found", ex.Message);
    }
}
