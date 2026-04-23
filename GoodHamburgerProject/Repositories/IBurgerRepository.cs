using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IBurgerRepository
    {
        Task<List<BurgerModel>> GetAllBurgersAsync();

        Task<BurgerModel?> GetBurgerByIdAsync(Guid id);

        Task<bool> CheckBurgerByNameAsync(string name);

        Task<BurgerModel> GetBurgerByNameAsync(string name);

        Task<BurgerModel> AddBurgerAsync(BurgerModel burger);

        Task<bool> UpdateBurgerAsync(BurgerModel burger);

        Task<bool> DeleteBurgerAsync(Guid id);
    }
}
