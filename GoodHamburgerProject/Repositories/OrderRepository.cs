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

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await context.Orders
                .Include(o => o.Items) 
                .ToListAsync();
        }

        public async Task<OrderModel?> GetOrderByIdAsync(Guid id)
        {
            return await context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Items) 
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await context.Orders
                .Include(o => o.Items) 
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return false;

            order.Active = false;

            foreach (var item in order.Items)
            {
                item.Active = false;
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOrderAsync(OrderModel order)
        {
            try
            {
                context.Orders.Update(order);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
