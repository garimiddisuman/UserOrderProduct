using Microsoft.EntityFrameworkCore;
using UserService.API.Application.DTOs;
using UserService.API.Domain;

namespace UserService.API.Application.Interfaces;

public interface IUserService
{
    User CreateUser(CreateUserRequest user);
    DbSet<User> GetAllUsers();
    User? GetUserById(int id);
    bool DeleteUserById(int id);
}