using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Services
{
    public class MenuService(AppDbContext context) : IMenuService
    {
        public async Task<MenuResponseDTO> GetMenuAsync()
        {
            var burgers = await context.Burgers
                .Where(b => b.Active)
                .Select(b => new MenuItemDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price
                })
                .ToListAsync();

            var accompaniments = await context.Accompaniments
                .Where(a => a.Active)
                .Select(a => new MenuItemDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Price = a.Price
                })
                .ToListAsync();

            return new MenuResponseDTO
            {
                Burgers = burgers,
                Accompaniments = accompaniments
            };
        }
    }
}