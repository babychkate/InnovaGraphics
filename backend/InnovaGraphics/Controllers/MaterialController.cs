using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Factory;
using InnovaGraphics.Utils.Factory.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IMaterialMetadataFetcher _metadataFetcher;

        public MaterialController(IMaterialService materialService, IMaterialMetadataFetcher metadataFetcher)
        {
            _materialService = materialService;
            _metadataFetcher = metadataFetcher;
        }


        [HttpGet("get-materials")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _materialService.GetAllAsync();
            return Ok(materials);
        }

        [HttpGet("get-materials-types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var materials = await _materialService.GetAllMaterialsTypes();
            return Ok(materials);
        }

        [HttpGet("get-materials-themes")]
        public async Task<IActionResult> GetAllThemes()
        {
            var materials = await _materialService.GetAllMaterialsThemes();
            return Ok(materials);
        }

        [HttpGet("get-material-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var material = await _materialService.GetByIdAsync(id);
            if (material == null)
                return NotFound(new { Message = $"Material with ID {id} not found." });

            return Ok(material);
        }

        [HttpGet("extract-youtube-id")]
        public IActionResult ExtractVideoIdFromUrl([FromQuery] string url)
        {
            var id = YouTubeUrlHelper.ExtractVideoId(url);
            if (id == null)
                return BadRequest(new { Message = "Invalid YouTube URL" });

            return Ok(new { VideoId = id });
        }

        [HttpPost("create-material")]
        public async Task<IActionResult> Create([FromBody] CreateMaterialDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _materialService.CreateAsync(dto);

            // Повертаємо 201 Created з location header
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("update-material-by/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<Material> patchDoc)
        {
            var result = await _materialService.UpdateAsync(id, patchDoc);
            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { result.Message });
        }

        [HttpDelete("delete-material-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _materialService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });

            return Ok(new { result.Message });
        }

    }
}
