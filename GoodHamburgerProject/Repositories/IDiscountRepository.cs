using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IDiscountRepository
    {
        Task<List<DiscountModel>> GetActiveDiscountsAsync();
    }
}
