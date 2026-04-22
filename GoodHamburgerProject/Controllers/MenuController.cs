using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IMenuService service) : ControllerBase
    {
        /// <summary>
        /// Retrieves the menu with all active burgers and accompaniments.
        /// </summary>
        /// <returns>A menu containing available burgers and accompaniments</returns>
        /// <response code="200">Menu successfully retrieved</response>
        [ProducesResponseType(typeof(MenuResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<MenuResponseDTO>> GetMenu()
        {
            var result = await service.GetMenuAsync();

            return Ok(result);
        }
    }
}
