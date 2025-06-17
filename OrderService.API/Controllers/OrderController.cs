using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.DTOs;
using OrderService.API.Application.Interfaces;
using OrderService.API.Domain;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/Orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = orderService.CreateOrder(request);
            return Created("api/Orders", order);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAllOrders()
    {
        var orders = orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public IActionResult GetOrderById(int id)
    {
        var order = orderService.GetOrderById(id);
        if (order == null)
            return NotFound(new { message = $"Order with ID {id} not found" });

        return Ok(order);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var deleted = orderService.DeleteOrder(id);
        if (!deleted)
            return NotFound(new { message = $"Order with ID {id} not found" });

        return NoContent();
    }
}