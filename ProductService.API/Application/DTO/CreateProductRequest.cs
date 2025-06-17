namespace ProductService.API.Application.DTO;

public class CreateProductRequest
{
    public required string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}
