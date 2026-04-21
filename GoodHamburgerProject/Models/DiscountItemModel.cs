using GoodHamburgerProject.Enums;

namespace GoodHamburgerProject.Models
{
    public class DiscountItemModel
    {
        public Guid Id { get; set; }

        public Guid DiscountId { get; set; }
        public DiscountModel Discount { get; set; } = null!;

        public Guid ProductId { get; set; }

        public ProductTypeEnum ProductType { get; set; }
    }
}
