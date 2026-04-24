using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request);

        Task<PagedResult<OrderResponseDTO>> GetAllOrdersPagedAsync(int page, int pageSize);

        Task<List<OrderResponseDTO>> GetAllOrdersAsync();
        Task<OrderResponseDTO> GetOrderByIdAsync(Guid id);
        Task<bool> DeleteOrderAsync(Guid id);

        Task<bool> UpdateOrderAsync(Guid id, UpdateOrderRequestDTO request);
    }
}