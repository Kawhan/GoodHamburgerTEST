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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



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

app.Run();
