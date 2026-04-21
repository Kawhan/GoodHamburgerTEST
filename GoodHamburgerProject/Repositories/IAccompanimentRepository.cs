

using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Repositories
{
    public interface IAccompanimentRepository
    {
        Task<List<AccompanimentModel>> GetAllAccompanimentsAsync();

        Task<AccompanimentModel?> GetAccompanimentByIdAsync(Guid id);

        Task<bool> CheckAccompanimentByNameAsync(string name);

        Task<AccompanimentModel> GetAccompanimentByNameAsync(string name);

        Task<AccompanimentModel> AddAccompanimentAsync(AccompanimentModel accompaniment);

        Task<bool> UpdateAccompanimentAsync(AccompanimentModel accompaniment);

        Task<bool> DeleteAccompanimentAsync(Guid id);
    }
}
