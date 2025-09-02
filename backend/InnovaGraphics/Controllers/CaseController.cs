using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _caseService;
        private readonly ICaseRepository _caseRepository;

        public CaseController(ICaseService caseService, ICaseRepository caseRepository)
        {
            _caseService = caseService;
            _caseRepository = caseRepository;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("create-case")]
        public async Task<ActionResult<Response>> CreateCase([FromBody] CreateCaseDto newCaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _caseService.CreateAsync(newCaseDto);

            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("get-all-cases")]
        public async Task<ActionResult<IEnumerable<Case>>> GetAllCases()
        {
            var cases = await _caseService.GetAllAsync();
            return Ok(cases);
        }

        [HttpGet("get-case-with-id/{id}")]
        public async Task<ActionResult<Case>> GetCaseByID(Guid id)
        {
            var casee = await _caseService.GetByIdAsync(id);

            if (casee == null)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Кейс не знайдено",
                });
            }

            return Ok(casee);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch("update-case/{id}")]
        public async Task<IActionResult> UpdateCase(Guid id, [FromBody] JsonPatchDocument<Case> patchDoc)
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

            var response = await _caseService.UpdateAsync(id, patchDoc);

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
        [HttpDelete("delete-case/{id}")]
        public async Task<IActionResult> DeleteCase(Guid id)
        {
            var response = await _caseService.DeleteAsync(id);

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
                    Message = "Кейс не знайдено",
                });
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }


        [Authorize(Roles = "Student")]
        [HttpGet("get-by-exercise-id/{id}")]
        public async Task<ActionResult<Case>> GetByExerciseID(Guid id)
        {
            var exercise = await _caseRepository.GetByExerciseIdAsync(id);
            return Ok(exercise);
        }
    }
}
