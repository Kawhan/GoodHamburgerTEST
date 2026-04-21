namespace GoodHamburgerProject.DTOs
{
    public class AccompanimentResponseDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
    }
}
