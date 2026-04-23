using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Mappers;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;

namespace GoodHamburgerProject.Services
{
    public class BurgerService(IBurgerRepository burgerRepository) : IBurgerService
    {
        public async Task<BurgerResponseDTO> AddBurgerAsync(CreateBurgerRequestDTO burger)
        {
            await VerifyCreateBurger(burger);


            var newBurger = new BurgerModel
            {
                Name = burger.Name,
                Price = burger.Price,
            };

            var response = await burgerRepository.AddBurgerAsync(newBurger);
            return response.ToDTO();
        }

        public async Task<bool> DeleteBurgerAsync(Guid id)
        {
            var result = await burgerRepository.GetBurgerByIdAsync(id);

            if (result is null)
            {
                throw new BurgerNotFound();
            }

            var status = await burgerRepository.DeleteBurgerAsync(id);

            return status;
        }

        public async Task<List<BurgerResponseDTO>> GetAllBurgersAsync()
        {
            var burgers = await burgerRepository.GetAllBurgersAsync();

            return burgers.Select(b=> b.ToDTO()).ToList();
        }

        public async Task<BurgerResponseDTO?> GetBurgerByIdAsync(Guid id)
        {
            var result = await burgerRepository.GetBurgerByIdAsync(id);

            if (result is null)
            {
                throw new BurgerNotFound();
            }

            return result.ToDTO();
        }

        public async Task<bool> ExistsBurgerByNameAsync(string name)
        {
            var result = await burgerRepository.CheckBurgerByNameAsync(name);
            return result;
        }

        public async Task<BurgerModel> GetBurgerByNameAsync(string name)
        {
            var result = await burgerRepository.GetBurgerByNameAsync(name);
            return result;
        }

        public async Task<bool> UpdateBurgerAsync(Guid id, UpdateBurgerRequestDTO burger)
        {
            await VerifyUpdateBurger(burger, id);  

            var existsBurger = await burgerRepository.GetBurgerByIdAsync(id);

            if (existsBurger is null)
            {
                throw new BurgerNotFound();
            }

            existsBurger.Name = burger.Name;
            existsBurger.Price = burger.Price;
            existsBurger.Active = burger.Active;

            var status = await burgerRepository.UpdateBurgerAsync(existsBurger);

            return status;
        }


        #region Aux Methods 
        private async Task VerifyCreateBurger(CreateBurgerRequestDTO burger) 
        {
            var result = await ExistsBurgerByNameAsync(burger.Name);

            if (result)
                throw new BurgerAlreadyExistsException();
        }

        private async Task VerifyUpdateBurger(UpdateBurgerRequestDTO burger, Guid id)
        {
            var existsBurgerResults = await GetAllBurgersAsync();

            var existsDuplicateBurger = existsBurgerResults.
                Where(b => b.Id != id && b.Name.ToLower() == burger.Name.ToLower() && b.Active).Any();

            if (existsDuplicateBurger)
                throw new BurgerAlreadyExistsException();
        }


        #endregion
    }
}
