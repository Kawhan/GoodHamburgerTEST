using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BurgerController(IBurgerService serviceBurger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<BurgerResponseDTO>>> GetAllBurgers()
        {
            return Ok(await serviceBurger.GetAllBurgersAsync());      
        }

        /// <summary>
        /// Retrieves a burger by its unique identifier.
        /// </summary>
        /// <param name="id">Burger unique identifier (GUID)</param>
        /// <returns>A burger object if found</returns>
        /// <response code="200">Returns the burger</response>
        /// <response code="404">Burger not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BurgerResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BurgerResponseDTO>> GetBurgerByGuid(Guid id)
        {
            var burguer = await serviceBurger.GetBurgerByIdAsync(id);
            return Ok(burguer);
        }

        [HttpPost]
        public async Task<ActionResult<BurgerResponseDTO>> AddBurger(CreateBurgerRequestDTO request)
        {
            var createdBurger = await serviceBurger.AddBurgerAsync(request);
            return CreatedAtAction(
                nameof(GetBurgerByGuid),
                new { id = createdBurger.Id },
                createdBurger
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBurger(Guid id, UpdateBurgerRequestDTO request)
        {
            var updated = await serviceBurger.UpdateBurgerAsync(id, request);
            return updated ? NoContent() : NotFound("No hamburgers were found with that ID.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBurger(Guid id) 
        { 
            var deleted = await serviceBurger.DeleteBurgerAsync(id);
            return deleted ? NoContent() : NotFound("No hamburgers were found with that ID.");
        }
    }
}
