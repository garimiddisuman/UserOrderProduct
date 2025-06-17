using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTO;
using ProductService.API.Application.Interfaces;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/Products")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        return Ok(service.GetAllProducts());
    }

    [HttpPost("AddProduct")]
    public IActionResult CreateProduct([FromBody] CreateProductRequest product)
    {
        var createdProduct = service.CreateProduct(product);
        return Created($"api/Product/{product}",createdProduct);
    }

    [HttpPut("UpdateProduct")]
    public IActionResult UpdateProduct([FromBody] UpdateProductRequest product)
    {
        var updatedProduct = service.UpdateProduct(product);
        return updatedProduct == null ? BadRequest("Mismatched product id") : Ok(updatedProduct);
    }

    [HttpDelete("DeleteProduct")]
    public IActionResult DeleteProduct(int id)
    {
        var deletedProduct = service.DeleteProduct(id);

        return deletedProduct ? Ok(deletedProduct) : NotFound("Product not found");
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = service.GetProductById(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPatch("{id}/reduce-stock/{count}")]
    public IActionResult ReduceProductQuantity(int id, int count)
    {
        try
        {
            service.ReduceStock(id, count);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
