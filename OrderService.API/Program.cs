using Microsoft.EntityFrameworkCore;
using OrderService.API.Application.Interfaces;
using OrderService.API.Application.Services;
using OrderService.API.Infrastructure;

namespace OrderService.API;

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
            options.UseNpgsql(builder.Configuration.GetSection("AppSettings")["db"]);
        });
        
        builder.Services.AddScoped<IOrderService, Application.Services.OrderService>();
        builder.Services.AddScoped<IProductClient, ProductClient>();
        
        builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:9000");
        });
        return builder.Build();
    }
}