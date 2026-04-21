using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class BurgerRepository(AppDbContext context) : IBurgerRepository
    {
        public async Task<BurgerModel> AddBurgerAsync(BurgerModel burger)
        {
            context.Burgers.Add(burger);
            await context.SaveChangesAsync();
            return new BurgerModel 
            { 
                Id = burger.Id,
                Name = burger.Name,
                Price = burger.Price,
                Active = burger.Active,
            };
        }

        public async Task<bool> DeleteBurgerAsync(Guid id)
        {
            var burger = await context.Burgers
                .Where(b => b.Id == id).FirstOrDefaultAsync();
            burger.Active = false;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BurgerModel>> GetAllBurgersAsync()
        {
            var burgers = await context.Burgers.ToListAsync();

            return burgers;
        }

        public async Task<BurgerModel?> GetBurguerByIdAsync(Guid id)
        {
            var burger = await context.Burgers
                .Where(b => b.Id == id).FirstOrDefaultAsync();

            return burger;
        }

        public async Task<bool> GetBurguerByNameAsync(string name)
        {
            var exists = await context.Burgers
                .Where(b => b.Name == name)
                .AnyAsync();

            return exists;
        }

        public async Task<bool> UpdateBurgerAsync(BurgerModel burger)
        {
            try
            {
                context.Burgers.Update(burger);
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex) 
            {
                throw new InvalidOperationException(ex.Message);
            }
            
        }
    }
}
