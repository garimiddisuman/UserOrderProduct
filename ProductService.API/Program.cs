using Microsoft.EntityFrameworkCore;
using ProductService.API.Application.Interfaces;
using ProductService.API.Infrastructure;

namespace ProductService.API;

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
        
        builder.Services.AddScoped<IProductService, Application.Services.ProductService>();
        return builder.Build();
    }
}
