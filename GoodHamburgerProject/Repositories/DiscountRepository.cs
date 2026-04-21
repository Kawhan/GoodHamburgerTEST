using GoodHamburgerProject.Data;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Repositories
{
    public class DiscountRepository(AppDbContext context) : IDiscountRepository
    {
        public async Task<List<DiscountModel>> GetActiveDiscountsAsync()
        {
            return await context.Discounts
                .Include(d => d.Items)
                .ToListAsync();
        }
    }
}
