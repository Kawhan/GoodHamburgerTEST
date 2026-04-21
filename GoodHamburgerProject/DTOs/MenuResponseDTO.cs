namespace GoodHamburgerProject.DTOs
{
    public class MenuResponseDTO
    {
        public List<MenuItemDTO> Burgers { get; set; } = new();

        public List<MenuItemDTO> Accompaniments { get; set; } = new();
    }
}
