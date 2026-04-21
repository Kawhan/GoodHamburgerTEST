

using GoodHamburgerProject.DTOs;

namespace GoodHamburgerProject.Services
{
    public interface IAccompanimentService
    {
        Task<List<AccompanimentResponseDTO>> GetAllAccompanimentsAsync();

        Task<AccompanimentResponseDTO?> GetAccompanimentByIdAsync(Guid id);

        Task<AccompanimentResponseDTO> AddAccompanimentAsync(CreateAccompanimentRequestDTO accompaniment);

        Task<bool> UpdateAccompanimentAsync(Guid id, UpdateAccompanimentRequestDTO accompaniment);

        Task<bool> DeleteAccompanimentAsync(Guid id);
    }
}
