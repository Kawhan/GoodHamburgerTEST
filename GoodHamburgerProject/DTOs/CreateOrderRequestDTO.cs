using GoodHamburgerProject.Enums;

namespace GoodHamburgerProject.DTOs
{
    public class CreateOrderRequestDTO
    {
        public List<OrderItemRequestDTO> Items { get; set; } = new();
    }

    public class OrderItemRequestDTO
    {
        public ProductTypeEnum ProductType { get; set; }

        public Guid ProductId { get; set; }
    }
}