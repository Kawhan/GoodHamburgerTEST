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

        public async Task<bool> DeleteAccompanimentAsync(Guid id)
        {
            var accompaniment = await context.Accompaniments
                .Where(ac => ac.Id == id)
                .FirstOrDefaultAsync() ?? throw new AccompanimentNotFound();
            accompaniment.Active = false;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<AccompanimentModel?> GetAccompanimentByIdAsync(Guid id)
        {
            var accompaniment = await context.Accompaniments
                .Where(ac => ac.Id == id)
                .FirstOrDefaultAsync();

            return accompaniment;
        }

        public async Task<bool> CheckAccompanimentByNameAsync(string name)
        {
            var exists = await context.Accompaniments
                .Where(ac => ac.Name.ToLower() == name.ToLower() && ac.Active)
                .AnyAsync();

            return exists;
        }

        public async Task<AccompanimentModel> GetAccompanimentByNameAsync(string name)
        {
            var accompaniment = await context.Accompaniments
                .Where(ac => ac.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync() ?? throw new AccompanimentNotFound();

            return accompaniment;
        }

        public async Task<List<AccompanimentModel>> GetAllAccompanimentsAsync()
        {
            var accompaniment = await context.Accompaniments
                .ToListAsync();

            return accompaniment;
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
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
