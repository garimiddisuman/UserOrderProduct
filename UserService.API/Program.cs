using Microsoft.EntityFrameworkCore;
using UserService.API.Application.Interfaces;
using UserService.API.Infrastructure;

namespace UserService.API;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplication app = CreateApp();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.Run();
    }

    public static WebApplication CreateApp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetSection("AppSettings")["db"]);
        });
        
        builder.Services.AddScoped<IUserService, Application.Services.UserService>();
        return builder.Build();
    }
}