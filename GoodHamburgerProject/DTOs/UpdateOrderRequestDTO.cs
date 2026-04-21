namespace GoodHamburgerProject.DTOs
{
    public class UpdateOrderRequestDTO
    {
        public List<OrderItemRequestDTO> Items { get; set; } = new();
    }
}
