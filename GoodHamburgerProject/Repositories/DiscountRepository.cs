using GoodHamburgerProject.Data;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class DiscountRepository(AppDbContext context) : IDiscountRepository
    {
        public async Task<List<DiscountModel>> GetActiveDiscountsAsync()
        {
            try
            {
                return await context.Discounts
                .Where(d => d.Active)
                .Include(d => d.Items)
                .ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new DatabaseException(
                    methodName: "GetActiveDiscountsAsync",
                    message: "Error retrieving active discounts from the database",
                    innerException: ex
                );
            }
        }
    }
}
