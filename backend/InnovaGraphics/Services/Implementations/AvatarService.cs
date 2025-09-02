using InnovaGraphics.Dtos;
using InnovaGraphics.Enums;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class AvatarService : IAvatarService
    {
        private readonly IRepository<ShopItem> _shopItemRepository;

        public AvatarService(
            IRepository<ShopItem> shopItemRepository)
        {
            _shopItemRepository = shopItemRepository;
        }

        public async Task<IEnumerable<Avatar>> GetAllAsync()
        {
            var allItems = await _shopItemRepository.GetAllAsync();
            return allItems.OfType<Avatar>();
        }

        public async Task<Avatar?> GetByIdAsync(Guid id)
        {
            var item = await _shopItemRepository.GetByIdAsync(id);
            return item as Avatar;
        }

        public async Task<Response> CreateAsync(CreateShopItemDto dto)
        {
            var validation = Validate(dto);
            if (!validation.Success)
                return validation;

            // Створюємо аватар
            var avatar = new Avatar
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Price = dto.Price,
                PhotoPath = dto.PhotoPath
            };

            await _shopItemRepository.AddAsync(avatar);

            return new Response
            {
                Success = true,
                Message = "Avatar created successfully",
                Data = avatar
            };
        }

        public async Task<Response> AddAsync(Avatar avatar)
        {
            if (avatar == null)
                return new Response { Success = false, Message = "Invalid avatar data" };

            await _shopItemRepository.AddAsync(avatar);
            return new Response { Success = true, Message = "Avatar added successfully" };
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Avatar> patchDoc)
        {
            if (patchDoc == null)
                return new Response { Success = false, Message = "Invalid patch document" };

            var entity = await _shopItemRepository.GetByIdAsync(id) as Avatar;
            if (entity == null)
                return new Response { Success = false, Message = "Avatar not found" };

            patchDoc.ApplyTo(entity);

            // Валідація після патчу
            if (string.IsNullOrWhiteSpace(entity.Name))
                return new Response { Success = false, Message = "Avatar name cannot be empty" };

            await _shopItemRepository.UpdateAsync(entity);

            return new Response { Success = true, Message = "Avatar updated successfully" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var entity = await _shopItemRepository.GetByIdAsync(id);
            if (entity is not Avatar)
                return new Response { Success = false, Message = "Avatar not found" };

            await _shopItemRepository.DeleteAsync(id);
            return new Response { Success = true, Message = "Avatar deleted successfully" };
        }

        private Response Validate(CreateShopItemDto dto)
        {
            if (dto.Type != ShopItemType.Avatar)
            {
                return new Response { Success = false, Message = "Type of ShopItem must be 'Avatar'" };
            }
            if (dto == null)
                return new Response { Success = false, Message = "Invalid data" };

            if (string.IsNullOrWhiteSpace(dto.Name))
                return new Response { Success = false, Message = "Name is required" };

            if (dto.Price <= 0)
                return new Response { Success = false, Message = "Price must be positive" };

            if (string.IsNullOrWhiteSpace(dto.PhotoPath))
                return new Response { Success = false, Message = "PhotoPath is required" };

            return new Response { Success = true };
        }
    }
}