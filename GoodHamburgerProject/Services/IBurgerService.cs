using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Services
{
    public interface IBurgerService
    {
        Task<List<BurgerResponseDTO>> GetAllBurgersAsync();

        Task<PagedResult<BurgerResponseDTO>> GetAllBurgersPagedAsync(int page, int page_size);

        Task<BurgerResponseDTO?> GetBurgerByIdAsync(Guid id);

        Task<BurgerResponseDTO> AddBurgerAsync(CreateBurgerRequestDTO burger);

        Task<bool> UpdateBurgerAsync(Guid id, UpdateBurgerRequestDTO burger);

        Task<bool> DeleteBurgerAsync(Guid id); 
    }
}
