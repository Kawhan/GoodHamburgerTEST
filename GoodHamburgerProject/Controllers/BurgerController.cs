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

        /// <summary>
        /// Creates a new burger in the system.
        /// </summary>
        /// <param name="request">Data required to create a new burger</param>
        /// <returns>The created burger</returns>
        /// <response code="201">Burger successfully created</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="409">Burger already exists</response>
        [ProducesResponseType(typeof(BurgerResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Updates an existing burger.
        /// </summary>
        /// <param name="id">Burger unique identifier (GUID)</param>
        /// <param name="request">Data to update the burger</param>
        /// <returns>No content if the update is successful</returns>
        /// <response code="204">Burger successfully updated</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Burger not found</response>
        /// <response code="409">Burger with the same name already exists</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut]
        public async Task<ActionResult> UpdateBurger(Guid id, UpdateBurgerRequestDTO request)
        {
            var updated = await serviceBurger.UpdateBurgerAsync(id, request);
            return updated ? NoContent() : NotFound("No burgers were found with that ID.");
        }

        /// <summary>
        /// Deletes an existing burger.
        /// </summary>
        /// <param name="id">Burger unique identifier (GUID)</param>
        /// <returns>No content if the deletion is successful</returns>
        /// <response code="204">Burger successfully deleted</response>
        /// <response code="404">Burger not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBurger(Guid id) 
        { 
            var deleted = await serviceBurger.DeleteBurgerAsync(id);
            return deleted ? NoContent() : NotFound("No burgers were found with that ID.");
        }
    }
}
