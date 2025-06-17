using Microsoft.EntityFrameworkCore;
using UserService.API.Application.DTOs;
using UserService.API.Application.Interfaces;
using UserService.API.Domain;
using UserService.API.Infrastructure;

namespace UserService.API.Application.Services;

public class UserService(AppDbContext context) : IUserService
{
    public User CreateUser(CreateUserRequest user)
    {
        var newUser = new User {Name = user.Name};
        context.Users.Add(newUser);
        context.SaveChanges();
        
        return newUser;
    }

    public DbSet<User> GetAllUsers()
    {
        return context.Users;
    }

    public User? GetUserById(int id)
    {
        return context.Users.Find(id);
    }

    public bool DeleteUserById(int id)
    {
        var user = GetUserById(id);
        if (user == null) return false;
        context.Users.Remove(user);
        context.SaveChanges();
        return true;
    }
}