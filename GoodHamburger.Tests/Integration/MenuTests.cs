using GoodHamburgerProject.Data;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburger.Tests.Integration
{
    public class MenuTests
    {
        [Fact]
        public async Task GetMenuAsync_ShouldReturnOnlyActiveItems()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            await connection.OpenAsync();

            try
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(connection)
                    .Options;

                await using var context = new AppDbContext(options);

                await context.Database.EnsureCreatedAsync();

                context.Burgers.RemoveRange(context.Burgers);
                context.Accompaniments.RemoveRange(context.Accompaniments);
                await context.SaveChangesAsync();

                context.Burgers.AddRange(
                    new BurgerModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "X Burger",
                        Price = 10,
                        Active = true
                    },
                    new BurgerModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Inactive Burger",
                        Price = 15,
                        Active = false
                    }
                );

                context.Accompaniments.Add(
                    new AccompanimentModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fries",
                        Price = 5,
                        Active = true
                    }
                );

                await context.SaveChangesAsync();

                var service = new MenuService(context);

                var result = await service.GetMenuAsync();

                Assert.Single(result.Burgers);
                Assert.Single(result.Accompaniments);

                var resultFirstBurger = result.Burgers.First();
                var resultFirstAccompaniment = result.Accompaniments.First();

                Assert.Equal("X Burger", resultFirstBurger.Name);
                Assert.Equal(10, resultFirstBurger.Price);

                Assert.Equal("Fries", resultFirstAccompaniment.Name);
                Assert.Equal(5, resultFirstAccompaniment.Price);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        [Fact]
        public async Task GetMenuAsync_NoData_ShouldReturnEmptyLists()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            await connection.OpenAsync();

            try
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(connection)
                    .Options;

                await using var context = new AppDbContext(options);

                await context.Database.EnsureCreatedAsync();

                context.Burgers.RemoveRange(context.Burgers);
                context.Accompaniments.RemoveRange(context.Accompaniments);
                await context.SaveChangesAsync();

                var service = new MenuService(context);

                var result = await service.GetMenuAsync();

                Assert.NotNull(result);
                Assert.Empty(result.Burgers);
                Assert.Empty(result.Accompaniments);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
