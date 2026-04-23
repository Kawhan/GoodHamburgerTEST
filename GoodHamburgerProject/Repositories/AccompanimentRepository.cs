using GoodHamburgerProject.Data;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace GoodHamburgerProject.Repositories
{
    public class AccompanimentRepository(AppDbContext context) : IAccompanimentRepository
    {
        public async Task<AccompanimentModel> AddAccompanimentAsync(AccompanimentModel accompaniment)
        {
            try
            {
                context.Accompaniments.Add(accompaniment);
                await context.SaveChangesAsync();
                return new AccompanimentModel
                {
                    Id = accompaniment.Id,
                    Name = accompaniment.Name,
                    Price = accompaniment.Price,
                    Active = accompaniment.Active
                };
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                        methodName: "AddAccompanimentAsync",
                        message: "Error add new accompaniment to the database",
                        innerException: ex
                );
            }
        }

        public async Task<bool> DeleteAccompanimentAsync(Guid id)
        {
            try
            {
                var accompaniment = await context.Accompaniments
                .Where(ac => ac.Id == id)
                .FirstOrDefaultAsync() ?? throw new AccompanimentNotFoundException();
                accompaniment.Active = false;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                       methodName: "DeleteAccompanimentAsync",
                       message: "Error delete a accompaniment to the database",
                       innerException: ex
               );
            }
        }

        public async Task<AccompanimentModel?> GetAccompanimentByIdAsync(Guid id)
        {
            try
            {
                var accompaniment = await context.Accompaniments
               .Where(ac => ac.Id == id)
               .FirstOrDefaultAsync();
                return accompaniment;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                       methodName: "GetAccompanimentByIdAsync",
                       message: "Error retrieving accompaniment from the database",
                       innerException: ex
               );
            }
        }

        public async Task<bool> CheckAccompanimentByNameAsync(string name)
        {
            try
            {
                var exists = await context.Accompaniments
                .Where(ac => ac.Name.ToLower() == name.ToLower() && ac.Active)
                .AnyAsync();
                return exists;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                        methodName: "CheckAccompanimentByNameAsync",
                        message: "Error check status by accompaniments name from the database",
                        innerException: ex
                );
            }
        }

        public async Task<AccompanimentModel> GetAccompanimentByNameAsync(string name)
        {
            try
            {
                var accompaniment = await context.Accompaniments
               .Where(ac => ac.Name.ToLower() == name.ToLower())
               .FirstOrDefaultAsync() ?? throw new AccompanimentNotFoundException();
                return accompaniment;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                        methodName: "GetAccompanimentByNameAsync",
                        message: "Error retrieving accompaniment by name from the database",
                        innerException: ex
                );
            }
        }

        public async Task<List<AccompanimentModel>> GetAllAccompanimentsAsync()
        {
            try
            {
                var accompaniment = await context.Accompaniments
                .ToListAsync();
                return accompaniment;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                        methodName: "GetAllAccompanimentsAsync",
                        message: "Error retrieving all accompaniments from the database",
                        innerException: ex
                );
            }
        }

        public async Task<bool> UpdateAccompanimentAsync(AccompanimentModel accompaniment)
        {
            try
            {
                context.Accompaniments.Update(accompaniment);
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex) 
            {
                throw new DatabaseException(
                        methodName: "UpdateAccompanimentAsync",
                        message: "Error updating accompaniment record in database",
                        innerException: ex
                );
            }
        }
    }
}
