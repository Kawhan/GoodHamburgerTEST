using GoodHamburgerProject.Data;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task<OrderModel> AddOrderAsync(OrderModel order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }
    }
}
