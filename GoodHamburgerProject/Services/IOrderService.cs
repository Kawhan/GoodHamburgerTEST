using GoodHamburgerProject.DTOs;

namespace GoodHamburgerProject.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request);
    }
}