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
        [HttpGet]
        public async Task<ActionResult<List<BurgerResponseDTO>>> GetAllAccompaniments()
        {
            return Ok(await serviceAccompaniment.GetAllAccompanimentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccompanimentResponseDTO>> GetAccompanimentByGuid(Guid id)
        {
            var accompaniment = await serviceAccompaniment.GetAccompanimentByIdAsync(id);

            if (accompaniment is null)
            {
                return NotFound("No accompaniment were found with that ID.");
            }

            return Ok(accompaniment);
        }

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

        [HttpPut]
        public async Task<ActionResult> UpdateAccompaniment(Guid id, UpdateAccompanimentRequestDTO request)
        {
            var updated = await serviceAccompaniment.UpdateAccompanimentAsync(id, request);
            return updated ? NoContent() : NotFound("No accompaniment were found with that ID.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccompaniment(Guid id)
        {
            var deleted = await serviceAccompaniment.DeleteAccompanimentAsync(id);
            return deleted ? NoContent() : NotFound("No accompaniment were found with that ID.");
        }
    }
}
