

using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Services
{
    public interface IAccompanimentService
    {
        Task<List<AccompanimentResponseDTO>> GetAllAccompanimentsAsync();

        Task<PagedResult<AccompanimentResponseDTO>> GetAllAccompanimentsPagedAsync(int page, int page_size);


        Task<AccompanimentResponseDTO?> GetAccompanimentByIdAsync(Guid id);

        Task<AccompanimentResponseDTO> AddAccompanimentAsync(CreateAccompanimentRequestDTO accompaniment);

        Task<bool> UpdateAccompanimentAsync(Guid id, UpdateAccompanimentRequestDTO accompaniment);

        Task<bool> DeleteAccompanimentAsync(Guid id);
    }
}
