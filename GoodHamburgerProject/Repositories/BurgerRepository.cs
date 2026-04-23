using GoodHamburgerProject.Data;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class BurgerRepository(AppDbContext context) : IBurgerRepository
    {
        public async Task<BurgerModel> AddBurgerAsync(BurgerModel burger)
        {
            try
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
            catch(Exception ex) 
            {
                throw new DatabaseException(
                    methodName: "AddBurgerAsync",
                    message: "Error add new burger to the database",
                    innerException: ex
                );
            }
        }

        public async Task<bool> DeleteBurgerAsync(Guid id)
        {
            try
            {
                var burger = await context.Burgers
                .Where(b => b.Id == id).FirstOrDefaultAsync() ?? throw new BurgerNotFound();
                burger.Active = false;
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "DeleteBurgerAsync",
                    message: "Error delete a burger to the database",
                    innerException: ex
                );
            }
        }

        public async Task<List<BurgerModel>> GetAllBurgersAsync()
        {
            try
            {
                var burgers = await context.Burgers.ToListAsync();
                return burgers;
            }
            catch (Exception ex) 
            {
                throw new DatabaseException(
                        methodName: "GetAllBurgersAsync",
                        message: "Error retrieving hamburgers from the database",
                        innerException: ex
                );
            }
        }

        public async Task<BurgerModel?> GetBurgerByIdAsync(Guid id)
        {
            try
            {
                var burger = await context.Burgers
                    .Where(b => b.Id == id).FirstOrDefaultAsync();
                return burger;
            } catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "GetBurgerByIdAsync",
                    message: "Error retrieving hamburger from the database",
                    innerException: ex
                );
            }
        }

        public async Task<bool> CheckBurgerByNameAsync(string name)
        {
            try 
            {
                var exists = await context.Burgers
               .Where(b => b.Name.ToLower() == name.ToLower() && b.Active)
               .AnyAsync();
                return exists;
            } catch (Exception ex)
            {
                throw new DatabaseException(
                    methodName: "CheckBurgerByNameAsync",
                    message: "Error check status by burger name from the database",
                    innerException: ex
                );
            }
        }

        public async Task<BurgerModel> GetBurgerByNameAsync(string name)
        {
            try
            {
                var burger = await context.Burgers
                .Where(b => b.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync() ?? throw new BurgerNotFound();
                return burger;
            }
            catch (Exception ex) 
            {
                throw new DatabaseException(
                    methodName: "GetBurgerByNameAsync",
                    message: "Error retrieving hamburger from the database",
                    innerException: ex
                );
            }
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
                throw new DatabaseException(
                    methodName: "UpdateBurgerAsync",
                    message: "Error updating burger record in database",
                    innerException: ex
                );
            }
            
        }

    }
}
