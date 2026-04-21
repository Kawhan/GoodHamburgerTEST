using System.ComponentModel.DataAnnotations;

namespace GoodHamburgerProject.Models
{
    public class AccompanimentModel
    {
        public Guid Id { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "he price must be greater than zero.")]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
    }
}
