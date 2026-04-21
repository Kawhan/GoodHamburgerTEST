using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;

public static class BurgerMapper
{
    public static BurgerResponseDTO ToDTO(this BurgerModel model)
    {
        return new BurgerResponseDTO
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Active = model.Active
        };
    }
}