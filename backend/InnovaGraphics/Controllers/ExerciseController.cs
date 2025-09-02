using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseService)
        {
            _exerciseRepository = exerciseService;
        }

        [HttpGet("get-all-exercises")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetAllCases()
        {
            var cases = await _exerciseRepository.GetAllAsync();
            return Ok(cases);
        }

        [HttpGet("get-exercise-with-id/{id}")]
        public async Task<ActionResult<Exercise>> GetCaseByID(Guid id)
        {
            var casee = await _exerciseRepository.GetByIdAsync(id);

            if (casee == null)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Вправу не знайдено",
                });
            }

            return Ok(casee);
        }

        [HttpGet("get-by-planet-id/{id}")]
        public async Task<ActionResult<Exercise>> GetByPlanetID(Guid id)
        {
            var planet = await _exerciseRepository.GetByPlanetIdAsync(id);
            return Ok(planet);
        }
    }
}
