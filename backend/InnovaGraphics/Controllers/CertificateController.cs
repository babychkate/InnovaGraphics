using Microsoft.AspNetCore.Mvc;
using InnovaGraphics.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace InnovaGraphics.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("template-image-url")]
        public async Task<IActionResult> GetCertificateTemplateImageUrl()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            var response = await _certificateService.GetTemplateImageUrl(userId);

            if (response.Success)
            {
                return Ok(new { response.Data });
            }
            else
            {
                return StatusCode(response.StatusCode, response.ValidationErrors);
            }
        }
    }
}
