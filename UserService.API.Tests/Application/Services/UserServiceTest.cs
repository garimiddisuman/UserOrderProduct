using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserService.API.Application.DTOs;
using UserService.API.Domain;
using UserService.API.Infrastructure;
using Xunit;

namespace UserService.API.Tests.Application.Services;

public class UserServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateUser_ShouldAddUser()
    {
        var context = GetDbContext();
        var service = new UserService.API.Application.Services.UserService(context);

        var request = new CreateUserRequest { Name = "Suman" };
        var result = service.CreateUser(request);

        Assert.NotNull(result);
        Assert.Equal("Suman", result.Name);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public void GetAllUsers_ShouldReturnAll()
    {
        var context = GetDbContext();
        var service = new UserService.API.Application.Services.UserService(context);
        context.Users.Add(new User { Name = "User1" });
        context.Users.Add(new User { Name = "User2" });
        context.SaveChanges();

        var users = service.GetAllUsers().ToList();

        Assert.Equal(2, users.Count);
    }

    [Fact]
    public void GetUserById_ShouldReturnUser_IfExists()
    {
        var context = GetDbContext();
        var user = new User { Name = "Test" };
        context.Users.Add(user);
        context.SaveChanges();

        var service = new UserService.API.Application.Services.UserService(context);
        var result = service.GetUserById(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public void GetUserById_ShouldReturnNull_IfNotExists()
    {
        var context = GetDbContext();
        var service = new UserService.API.Application.Services.UserService(context);

        var result = service.GetUserById(999);

        Assert.Null(result);
    }

    [Fact]
    public void DeleteUserById_ShouldDelete_IfExists()
    {
        var context = GetDbContext();
        var user = new User { Name = "ToDelete" };
        context.Users.Add(user);
        context.SaveChanges();

        var service = new UserService.API.Application.Services.UserService(context);
        var result = service.DeleteUserById(user.Id);

        Assert.True(result);
        Assert.Null(context.Users.Find(user.Id));
    }

    [Fact]
    public void DeleteUserById_ShouldReturnFalse_IfNotExists()
    {
        var context = GetDbContext();
        var service = new UserService.API.Application.Services.UserService(context);

        var result = service.DeleteUserById(123);

        Assert.False(result);
    }
}
