namespace GoodHamburgerProject.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public List<OrderItemModel> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }

        public bool Active { get; set; } = true; 
    }
}
