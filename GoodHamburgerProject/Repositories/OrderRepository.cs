using GoodHamburgerProject.Data;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task<OrderModel> AddOrderAsync(OrderModel order)
        {
            try
            {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "AddOrderAsync",
                    message: "Error add new order to the database",
                    innerException: ex
                );
            }
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            try
            {
                return await context.Orders
                .Include(o => o.Items)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                   methodName: "GetAllOrdersAsync",
                   message: "Error retrieving all orders from the database",
                   innerException: ex
               );
            }
        }

        public async Task<OrderModel?> GetOrderByIdAsync(Guid id)
        {
            try
            {
                return await context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Items)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "GetOrderByIdAsync",
                    message: "Error retrieving order from the database",
                    innerException: ex
                );
            }
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "DeleteOrderAsync",
                    message: "Error delete a order to the database",
                    innerException: ex
                );
            }
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
                throw new DatabaseException(
                        methodName: "UpdateOrderAsync",
                        message: "Error updating order record in database",
                        innerException: ex
                );
            }
        }
    }
}
