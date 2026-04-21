using System.ComponentModel.DataAnnotations;

namespace GoodHamburgerProject.DTOs
{
    public class BurgerResponseDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
    }
}
