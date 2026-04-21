using GoodHamburgerProject.Enums;

namespace GoodHamburgerProject.DTOs
{
    public class OrderItemResponseDTO
    {
        public Guid Id { get; set; }

        public ProductTypeEnum ProductType { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool Active { get; set; }
    }
}
