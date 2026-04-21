using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IBurgerRepository
    {
        Task<List<BurgerModel>> GetAllBurgersAsync();

        Task<BurgerModel?> GetBurguerByIdAsync(Guid id);

        Task<bool> CheckBurguerByNameAsync(string name);

        Task<BurgerModel> GetBurguerByNameAsync(string name);

        Task<BurgerModel> AddBurgerAsync(BurgerModel burger);

        Task<bool> UpdateBurgerAsync(BurgerModel burger);

        Task<bool> DeleteBurgerAsync(Guid id);
    }
}
