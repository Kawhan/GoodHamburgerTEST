using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<BurgerModel> Burgers => Set<BurgerModel>();
        public DbSet<AccompanimentModel> Accompaniments => Set<AccompanimentModel>();

        public DbSet<OrderModel> Orders => Set<OrderModel>();
        public DbSet<OrderItemModel> OrderItems => Set<OrderItemModel>();

        public DbSet<DiscountModel> Discounts => Set<DiscountModel>();
        public DbSet<DiscountItemModel> DiscountItems => Set<DiscountItemModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}