using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Services
{
    public interface IBurgerService
    {
        Task<List<BurgerResponseDTO>> GetAllBurgersAsync();

        Task<BurgerResponseDTO?> GetBurgerByIdAsync(Guid id);

        Task<BurgerResponseDTO> AddBurgerAsync(CreateBurgerRequestDTO burger);

        Task<bool> UpdateBurgerAsync(Guid id, UpdateBurgerRequestDTO burger);

        Task<bool> DeleteBurgerAsync(Guid id); 
    }
}
