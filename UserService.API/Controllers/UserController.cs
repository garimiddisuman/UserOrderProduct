using Microsoft.AspNetCore.Mvc;
using UserService.API.Application.DTOs;
using UserService.API.Application.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/Users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest user)
    {
        userService.CreateUser(user);
        return Created("api/Users", user);
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var allUsers = userService.GetAllUsers();
        return Ok(allUsers);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = userService.GetUserById(id);
        return user == null ? NotFound("User not found") : Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteById(int id)
    {
        var user = userService.DeleteUserById(id);
        return user ? NoContent() : NotFound("User not found");
    }
}
