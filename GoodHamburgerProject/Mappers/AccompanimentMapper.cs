using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

namespace GoodHamburgerProject.Mappers
{
    public static class AccompanimentMapper
    {
        public static AccompanimentResponseDTO ToDTO(this AccompanimentModel model)
        {
            return new AccompanimentResponseDTO
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Active = model.Active
            };
        }
    }
}
