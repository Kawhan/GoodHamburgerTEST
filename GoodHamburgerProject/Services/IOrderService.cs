using GoodHamburgerProject.DTOs;

namespace GoodHamburgerProject.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request);
        Task<List<OrderResponseDTO>> GetAllOrdersAsync();
        Task<OrderResponseDTO> GetOrderByIdAsync(Guid id);
        Task<bool> DeleteOrderAsync(Guid id);

        Task<bool> UpdateOrderAsync(Guid id, UpdateOrderRequestDTO request);
    }
}