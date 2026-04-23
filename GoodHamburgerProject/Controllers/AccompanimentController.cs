using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccompanimentController(IAccompanimentService serviceAccompaniment) : ControllerBase
    {

        /// <summary>
        /// Retrieves all accompaniments.
        /// </summary>
        /// <returns>A list of accompaniments</returns>
        /// <response code="200">Returns the list of accompaniments</response>
        /// <response code="500">Database Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<AccompanimentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<BurgerResponseDTO>>> GetAllAccompaniments()
        {
            return Ok(await serviceAccompaniment.GetAllAccompanimentsAsync());
        }


        /// <summary>
        /// Retrieves an accompaniment by its unique identifier.
        /// </summary>
        /// <param name="id">Accompaniment unique identifier (GUID)</param>
        /// <returns>An accompaniment if found</returns>
        /// <response code="200">Returns the accompaniment</response>
        /// <response code="404">Accompaniment not found</response>
        /// <response code="500">Database Error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccompanimentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccompanimentResponseDTO>> GetAccompanimentByGuid(Guid id)
        {
            var accompaniment = await serviceAccompaniment.GetAccompanimentByIdAsync(id);
            return Ok(accompaniment);
        }

        /// <summary>
        /// Creates a new accompaniment.
        /// </summary>
        /// <param name="request">Accompaniment data to be created</param>
        /// <returns>The created accompaniment</returns>
        /// <response code="201">Accompaniment successfully created</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="409">Accompaniment already exists</response>
        /// <response code="500">Database Error</response>
        [HttpPost]
        [ProducesResponseType(typeof(AccompanimentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<AccompanimentResponseDTO>> AddAccompaniment(CreateAccompanimentRequestDTO request)
        {
            var createdAccompaniment = await serviceAccompaniment.AddAccompanimentAsync(request);
            return CreatedAtAction(
                nameof(GetAccompanimentByGuid),
                new { id = createdAccompaniment.Id },
                createdAccompaniment
            );
        }

        /// <summary>
        /// Updates an existing accompaniment.
        /// </summary>
        /// <param name="id">Accompaniment unique identifier</param>
        /// <param name="request">Updated accompaniment data</param>
        /// <returns>No content if update is successful</returns>
        /// <response code="204">Accompaniment successfully updated</response>
        /// <response code="404">Accompaniment not found</response>
        /// <response code="409">Accompaniment already exists</response>
        /// <response code="500">Database Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult> UpdateAccompaniment(Guid id, UpdateAccompanimentRequestDTO request)
        {
            var updated = await serviceAccompaniment.UpdateAccompanimentAsync(id, request);
            return updated ? NoContent() : NotFound("No accompaniment were found with that ID.");
        }


        /// <summary>
        /// Deletes (deactivates) an accompaniment by its ID.
        /// </summary>
        /// <param name="id">Accompaniment unique identifier</param>
        /// <returns>No content if deletion is successful</returns>
        /// <response code="204">Accompaniment successfully deleted</response>
        /// <response code="404">Accompaniment not found</response>
        /// <response code="500">Database Error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAccompaniment(Guid id)
        {
            var deleted = await serviceAccompaniment.DeleteAccompanimentAsync(id);
            return deleted ? NoContent() : NotFound("No accompaniment were found with that ID.");
        }
    }
}
