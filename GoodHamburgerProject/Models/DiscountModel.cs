namespace GoodHamburgerProject.Models
{
    public class DiscountModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Percentage { get; set; }

        public List<DiscountItemModel> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        public bool Active { get; set; } = true;
    }
}
