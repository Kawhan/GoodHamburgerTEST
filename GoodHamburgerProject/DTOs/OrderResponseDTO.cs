namespace GoodHamburgerProject.DTOs
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Active { get; set; }

        public List<OrderItemResponseDTO> Items { get; set; } = new();

    }
}