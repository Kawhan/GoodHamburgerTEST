using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Mappers
{
    public static class OrderMapper
    {
        public static OrderResponseDTO ToDTO(this OrderModel order)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                CreatedAt = order.CreatedAt
            };
        }
    }
}