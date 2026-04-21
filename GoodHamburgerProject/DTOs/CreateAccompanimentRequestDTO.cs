namespace GoodHamburgerProject.DTOs
{
    public class CreateAccompanimentRequestDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }

        public bool Activate { get; set; } = true;
    }
}
