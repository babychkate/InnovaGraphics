using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InnovaGraphics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Student")]
        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateUser([FromBody] JsonPatchDocument<User> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "JsonPatchDocument не може бути порожнім."
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            var response = await _userService.UpdateUserAsync(userId, patchDocument);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case StatusCodes.Status400BadRequest:
                        return BadRequest(response);
                    case StatusCodes.Status404NotFound:
                        return NotFound(response);
                    default:
                        return StatusCode(response.StatusCode, response);
                }
            }
        }
    }
}