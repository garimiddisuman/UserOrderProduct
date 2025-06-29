namespace ProductService.API.Application.DTO;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Price { get; set; }
    public int? Quantity { get; set; }
}