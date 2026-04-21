namespace GoodHamburgerProject.DTOs
{
    public class MenuItemDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
