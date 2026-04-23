using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Enums;
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

        public async Task<bool> DeleteOrderAsync(OrderModel order)
        {
            try
            {
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

        public async Task<OrderProductsData> GetProductsDataAsync(OrderModel order)
        {
            var burgerIds = order.Items
                .Where(i => i.ProductType == ProductTypeEnum.Burger)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var accompanimentIds = order.Items
                .Where(i => i.ProductType != ProductTypeEnum.Burger)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var burgers = await context.Burgers
                .Where(b => burgerIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id);

            var accompaniments = await context.Accompaniments
                .Where(a => accompanimentIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id);

            return new OrderProductsData
            {
                Burgers = burgers,
                Accompaniments = accompaniments
            };
        }
    }
}
