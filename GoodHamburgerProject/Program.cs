using GoodHamburgerProject.Data;
using GoodHamburgerProject.Repositories;
using GoodHamburgerProject.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);



builder.Services.AddScoped<IBurgerService, BurgerService>();
builder.Services.AddScoped<IBurgerRepository, BurgerRepository>();

builder.Services.AddScoped<IAccompanimentService, AccompanimentService>();
builder.Services.AddScoped<IAccompanimentRepository, AccompanimentRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var retries = 10;

    while (retries > 0)
    {
        try
        {
            logger.LogInformation("Applying database migrations...");
            db.Database.Migrate();
            logger.LogInformation("Database ready.");
            break;
        }
        catch (Exception ex)
        {
            retries--;

            logger.LogWarning(ex, "Error connecting to DB. Retries left: {Retries}", retries);

            if (retries == 0)
                throw;

            await Task.Delay(3000);
        }
    }
}


app.Run();
