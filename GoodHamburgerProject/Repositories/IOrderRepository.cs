using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderModel> AddOrderAsync(OrderModel order);
    }
}
