using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IMenuService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<MenuResponseDTO>> GetMenu()
        {
            var result = await service.GetMenuAsync();

            return Ok(result);
        }
    }
}
