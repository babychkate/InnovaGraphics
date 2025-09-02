using Microsoft.AspNetCore.Mvc;
using InnovaGraphics.Data;   
using Microsoft.EntityFrameworkCore; 

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly AppDbContext _dbContext; 

        public ImagesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       [HttpGet("{imageId:guid}")]
        [Produces("image/jpeg", "image/png", "image/gif", "application/octet-stream")]
        public async Task<IActionResult> GetImageById(Guid imageId)
        {
            var image = await _dbContext.Image
                                        .AsNoTracking() 
                                        .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null || image.Content == null)
            {
                return NotFound();
            }

            string contentType;
            var fileExtension = Path.GetExtension(image.Name)?.ToLowerInvariant(); // Використовуємо System.IO.Path

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".webp":
                    contentType = "image/webp";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return File(image.Content, contentType);
        }

    }
}