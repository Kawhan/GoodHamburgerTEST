namespace GoodHamburgerProject.DTOs
{
    public class UpdateAccompanimentRequestDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public bool Active { get; set; }
    }
}
