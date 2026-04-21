using GoodHamburgerProject.DTOs;

namespace GoodHamburgerProject.Services
{
    public interface IMenuService
    {
        Task<MenuResponseDTO> GetMenuAsync();
    }
}
