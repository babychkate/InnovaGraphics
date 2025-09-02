using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanetController : ControllerBase
    {
        private readonly IPlanetService _planetService;

        public PlanetController(IPlanetService planetService)
        {
            _planetService = planetService;
        }

        [Authorize(Roles = "Teacher")]
        //CREATE
        [HttpPost("create-planet")]
        public async Task<ActionResult<Response>> CreatePlanet([FromBody] CreatePlanetDto newPlanetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _planetService.CreatePlanetAsync(newPlanetDto);

            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        //READ
        [HttpGet("get-all-planets")]
        public async Task<ActionResult<IEnumerable<Planet>>> GetAllPlanets()
        {
            var planets = await _planetService.GetAllAsync();
            return Ok(planets);
        }

        [HttpGet("get-planet-with-id/{id}")]
        public async Task<ActionResult<Planet>> GetPlanetByID(Guid id)
        {
            var planet = await _planetService.GetByIdAsync(id);

            if (planet == null)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Planet not found",
                });
            }

            return Ok(planet);
        }


        [Authorize(Roles = "Teacher")]
        //UPDATE
        [HttpPatch("update-planet/{id}")]
        public async Task<IActionResult> UpdatePlanet(Guid id, [FromBody] JsonPatchDocument<Planet> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Update data is empty",
                });
            }

            var response = await _planetService.UpdateAsync(id, patchDoc);

            if (response.Success)
            {
                return Ok();
            }
            else if (response.StatusCode == 404)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Planet not found",
                });
            }
            else
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Update data is empty",
                });
            }
        }

        [Authorize(Roles = "Teacher")]
        //DELETE
        [HttpDelete("delete-planet/{id}")]
        public async Task<IActionResult> DeletePlanet(Guid id)
        {
            var response = await _planetService.DeleteAsync(id);

            if (response.Success)
            {
                return Ok(response); // Зазвичай 200 OK з повідомленням про успіх
            }
            else if (response.StatusCode == 404)
            {
                return NotFound(new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Planet not found",
                });
            }
            else
            {
                return StatusCode(response.StatusCode, response); // Інші помилки
            }
        }

        // GET ALL TOPICS
        [HttpGet("get-all-planet-topics")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllPlanetTopics()
        {
            var topics = await _planetService.GetAllPlanetTopicsAsync();
            if(topics != null)
            {
                return Ok(topics);
            }
            return NotFound(new Response
            {
                Success = false,
                StatusCode = 404,
                Message = "Topics not found",
            });
        }

        // GET ALL SUBTOPICS
        [HttpGet("get-all-planet-subtopics")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllPlanetSubTopics()
        {
            var subTopics = await _planetService.GetAllPlanetSubTopicsAsync();
            if (subTopics != null)
            {
                return Ok(subTopics);
            }
            return NotFound(new Response
            {
                Success = false,
                StatusCode = 404,
                Message = "Subtopics not found",
            });
        }

        // GET ALL NAMES
        [HttpGet("get-all-planet-names")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllNames()
        {
            var names = await _planetService.GetAllPlanetNamesAsync();
            if (names != null)
            {
                return Ok(names);
            }
            return NotFound(new Response
            {
                Success = false,
                StatusCode = 404,
                Message = "Names not found",
            });
        }

    }
}
