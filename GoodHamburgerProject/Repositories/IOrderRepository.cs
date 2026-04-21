using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderModel> AddOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel?> GetOrderByIdAsync(Guid id);
        Task<bool> DeleteOrderAsync(Guid id);

        Task<bool> UpdateOrderAsync(OrderModel order);
    }
}
