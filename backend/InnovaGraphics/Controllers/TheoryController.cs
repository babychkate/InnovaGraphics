using Microsoft.AspNetCore.Mvc;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Interactions;
using InnovaGraphics.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheoryController : ControllerBase
    {
        private readonly ITheoryService _theoryService;
        private readonly ITheoryRepository _theoryRepository;


        public TheoryController(ITheoryService theoryService, ITheoryRepository theoryRepository)
        {
            _theoryService = theoryService;
            _theoryRepository = theoryRepository;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("create-theory")]
        public async Task<ActionResult<Response>> CreateTheory([FromBody] CreateTheoryDto newTheoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _theoryService.CreateAsync(newTheoryDto);

            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("get-all-theories")]
        public async Task<ActionResult<IEnumerable<Theory>>> GetAllTheories()
        {
            var theories = await _theoryService.GetAllAsync();
            return Ok(theories);
        }

        [HttpGet("get-theory-with-id/{id}")]
        public async Task<ActionResult<Theory>> GetTheoryByID(Guid id)
        {
            var theory = await _theoryService.GetByIdAsync(id);

            if (theory == null)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Теорії не знайдено",
                });
            }

            return Ok(theory);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch("update-theory/{id}")]
        public async Task<IActionResult> UpdateTheory(Guid id, [FromBody] JsonPatchDocument<Theory> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Дані для оновлення не можуть бути пустими.",
                });
            }

            var response = await _theoryService.UpdateAsync(id, patchDoc);

            if (response.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Дані для оновлення не можуть бути пустими.",
                });
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("delete-theory/{id}")]
        public async Task<IActionResult> DeleteTheory(Guid id)
        {
            var response = await _theoryService.DeleteAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            else if (response.StatusCode == 404)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Теорії не знайдено",
                });
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("get-by-planet-id/{id}")]
        public async Task<ActionResult<Theory>> GetByPlanetID(Guid id)
        {
            var planet = await _theoryRepository.GetByPlanetIdAsync(id);
            return Ok(planet);
        }
    }
}