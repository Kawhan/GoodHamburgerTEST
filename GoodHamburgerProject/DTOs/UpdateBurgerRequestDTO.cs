namespace GoodHamburgerProject.DTOs
{
    public class UpdateBurgerRequestDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public bool Active { get; set; } 
    }
}
