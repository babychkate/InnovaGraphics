using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopItemsController : ControllerBase
    {
        private readonly IAvatarService _avatarService;
        private readonly IBackgroundService _backgroundService;
        private readonly IPurchaseService _purchaseService;

        public ShopItemsController(IAvatarService avatarService, 
                                   IPurchaseService purchaseService, 
                                   IBackgroundService backgroundService)
        {
            _avatarService = avatarService;
            _backgroundService = backgroundService;
            _purchaseService = purchaseService;
        }

        [HttpGet("get-all-avatars")]
        public async Task<ActionResult<IEnumerable<Avatar>>> GetAllAvatars()
        {
            var avatars = await _avatarService.GetAllAsync();
            return Ok(avatars);
        }

        [HttpGet("get-avatar-by/{id}")]
        public async Task<ActionResult<Avatar>> GetAvatarById(Guid id)
        {
            var avatar = await _avatarService.GetByIdAsync(id);
            if (avatar == null)
                return NotFound(new { Message = "Avatar not found" });

            return Ok(avatar);
        }

        [HttpPost("create-avatar")]
        public async Task<ActionResult<Response>> CreateAvatar([FromBody] CreateShopItemDto shopItemDto)
        {
            var response = await _avatarService.CreateAsync(shopItemDto);
            if (!response.Success)
                return BadRequest(new { response.Message });

            return Ok(response);
        }

        [HttpPatch("update-avatar/{id}")]
        public async Task<ActionResult<Response>> UpdateAvatar(Guid id, [FromBody] JsonPatchDocument<Avatar> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(new { Message = "Invalid patch document" });

            var response = await _avatarService.UpdateAsync(id, patchDoc);
            if (!response.Success)
                return NotFound(new { response.Message });

            return Ok(response);
        }

        [HttpDelete("delete-avatar/{id}")]
        public async Task<ActionResult<Response>> DeleteAvatar(Guid id)
        {
            var response = await _avatarService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(new { response.Message });

            return Ok(response);
        }



        [HttpGet("get-all-backgrounds")]
        public async Task<ActionResult<IEnumerable<Background>>> GetAllBackgrounds()
        {
            var avatars = await _backgroundService.GetAllAsync();
            return Ok(avatars);
        }

        [HttpGet("get-background-by/{id}")]
        public async Task<ActionResult<Background>> GetBackgroundById(Guid id)
        {
            var avatar = await _backgroundService.GetByIdAsync(id);
            if (avatar == null)
                return NotFound(new { Message = "Avatar not found" });

            return Ok(avatar);
        }

        [HttpPost("create-background")]
        public async Task<ActionResult<Response>> CreateBackground([FromBody] CreateShopItemDto shopItemDto)
        {
            var response = await _backgroundService.CreateAsync(shopItemDto);
            if (!response.Success)
                return BadRequest(new { response.Message });

            return Ok(response);
        }

        [HttpPatch("update-background/{id}")]
        public async Task<ActionResult<Response>> UpdateBackground(Guid id, [FromBody] JsonPatchDocument<Background> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(new { Message = "Invalid patch document" });

            var response = await _backgroundService.UpdateAsync(id, patchDoc);
            if (!response.Success)
                return NotFound(new { response.Message });

            return Ok(response);
        }

        [HttpDelete("delete-background/{id}")]
        public async Task<ActionResult<Response>> DeletBackground(Guid id)
        {
            var response = await _backgroundService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(new { response.Message });

            return Ok(response);
        }


        [HttpPost("buy-item")]
        public async Task<IActionResult> BuyItem([FromBody] PurchaseDto request)
        {
            var result = await _purchaseService.BuyItemAsync(request.UserId, request.ShopItemId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("buy-item-by-planet-id/{id}")]
        public async Task<IActionResult> BuyItemByPlanetId([FromBody] PurchaseDto request)
        {
            var result = await _purchaseService.BuyItemAsync(request.UserId, request.ShopItemId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("get-all-purchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllAsync();
            return Ok(purchases);
        }

        [HttpGet("get-user-purchases/{userId}")]
        public async Task<IActionResult> GetPurchasesByUserId(Guid userId)
        {
            var purchases = await _purchaseService.GetByUserIdAsync(userId);
            return Ok(purchases);
        }

        [HttpGet("get-purchase/{id}")]
        public async Task<IActionResult> GetPurchaseById(Guid id)
        {
            var purchase = await _purchaseService.GetByIdAsync(id);
            if (purchase == null)
                return NotFound(new { Message = "Purchase not found" });

            return Ok(purchase);
        }
    }
}
