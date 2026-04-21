using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;

namespace GoodHamburgerProject.Services
{
    public class BurgerService(IBurgerRepository burgerRepository) : IBurgerService
    {
        public async Task<BurgerResponseDTO> AddBurgerAsync(CreateBurgerRequestDTO burger)
        {
            await verifyCreateBurger(burger);


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
            var result = await burgerRepository.GetBurguerByIdAsync(id);

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
            var result = await burgerRepository.GetBurguerByIdAsync(id);

            if (result is null)
            {
                throw new BurgerNotFound();
            }

            return result.ToDTO();
        }

        public async Task<bool> ExistsBurgerByNameAsync(string name)
        {
            var result = await burgerRepository.GetBurguerByNameAsync(name);
            return result;
        }

        public async Task<bool> UpdateBurgerAsync(Guid id, UpdateBurgerRequestDTO burger)
        {
            var existsBurger = await burgerRepository.GetBurguerByIdAsync(id);

            if (existsBurger is null)
            {
                throw new BurgerNotFound();
            }

            existsBurger.Name = burger.Name;
            existsBurger.Price = burger.Price;

            var status = await burgerRepository.UpdateBurgerAsync(existsBurger);

            return status;
        }


        #region Aux Methods 
        private async Task verifyCreateBurger(CreateBurgerRequestDTO burger) 
        {
            var result = await burgerRepository.GetBurguerByNameAsync(burger.Name);

            if (result)
                throw new BurgerAlreadyExistsException();
        }


        #endregion
    }
}
