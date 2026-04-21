using GoodHamburgerProject.Enums;

namespace GoodHamburgerProject.Models
{
    public class OrderItemModel
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; } = null!;

        public Guid AccompanimentId { get; set; }

        public ProductTypeEnum ProductType { get; set; }

        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
    }
}
