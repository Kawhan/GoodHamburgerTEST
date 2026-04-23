using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.DTOs
{
    public class OrderProductsData
    {
        public Dictionary<Guid, BurgerModel> Burgers { get; set; } = new();
        public Dictionary<Guid, AccompanimentModel> Accompaniments { get; set; } = new();
    }
}
