namespace GoodHamburgerProject.DTOs
{
    public class CreateBurgerRequestDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }

        public bool Activate { get; set; } = true;
    }
}
