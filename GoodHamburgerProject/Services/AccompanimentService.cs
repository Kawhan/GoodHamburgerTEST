using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Mappers;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;

namespace GoodHamburgerProject.Services
{
    public class AccompanimentService(IAccompanimentRepository accompanimentRepository) : IAccompanimentService
    {
        public async Task<AccompanimentResponseDTO> AddAccompanimentAsync(CreateAccompanimentRequestDTO accompaniment)
        {
            await VerifyCreateAccompaniment(accompaniment);

            var newAccompaniment = new AccompanimentModel
            {
                Name = accompaniment.Name,
                Price = accompaniment.Price,
            };

            var response = await accompanimentRepository.AddAccompanimentAsync(newAccompaniment);
            return response.ToDTO();
        }

        public async Task<bool> DeleteAccompanimentAsync(Guid id)
        {
            var result = await accompanimentRepository.GetAccompanimentByIdAsync(id);

            if (result is null)
            {
                throw new AccompanimentNotFoundException();
            }

            var status = await accompanimentRepository.DeleteAccompanimentAsync(id);

            return status;
        }

        public async Task<AccompanimentResponseDTO?> GetAccompanimentByIdAsync(Guid id)
        {
            var result = await accompanimentRepository.GetAccompanimentByIdAsync(id);

            if (result is null)
            {
                throw new AccompanimentNotFoundException();
            }

            return result.ToDTO();
        }

        public async Task<List<AccompanimentResponseDTO>> GetAllAccompanimentsAsync()
        {
            var accompaniments = await accompanimentRepository.GetAllAccompanimentsAsync();

            return accompaniments.Select(ac => ac.ToDTO()).ToList();
        }

        public async Task<bool> UpdateAccompanimentAsync(Guid id, UpdateAccompanimentRequestDTO accompaniment)
        {
            await VerifyUpdateAccompaniment(accompaniment, id);

            var existsAccompaniment = await accompanimentRepository.GetAccompanimentByIdAsync(id);

            if (existsAccompaniment is null)
            {
                throw new AccompanimentNotFoundException();
            }

            existsAccompaniment.Name = accompaniment.Name;
            existsAccompaniment.Price = accompaniment.Price;
            existsAccompaniment.Active = accompaniment.Active;

            var status = await accompanimentRepository.UpdateAccompanimentAsync(existsAccompaniment);

            return status;
        }

        public async Task<bool> ExistsAccompanimentByNameAsync(string name)
        {
            var result = await accompanimentRepository.CheckAccompanimentByNameAsync(name);
            return result;
        }

        public async Task<AccompanimentModel> GetAccompanimentByNameAsync(string name)
        {
            var result = await accompanimentRepository.GetAccompanimentByNameAsync(name);
            return result;
        }


        #region aux Methods
        private async Task VerifyCreateAccompaniment(CreateAccompanimentRequestDTO accompaniment)
        {
            var result = await ExistsAccompanimentByNameAsync(accompaniment.Name);

            if (result)
                throw new AccompanimentAlreadyExistsException();
        }

        private async Task VerifyUpdateAccompaniment(UpdateAccompanimentRequestDTO accompaniment, Guid id)
        {
            var existsAccompanimentsResults = await GetAllAccompanimentsAsync();

            var existsDuplicateAccompaniment = existsAccompanimentsResults.
                Where(ac => ac.Id != id && ac.Name.ToLower() == accompaniment.Name.ToLower() && ac.Active).Any();

            if (existsDuplicateAccompaniment)
                throw new AccompanimentAlreadyExistsException();
        }

        #endregion
    }
}
