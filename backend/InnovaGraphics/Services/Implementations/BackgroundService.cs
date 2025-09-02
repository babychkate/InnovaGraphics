using InnovaGraphics.Dtos;
using InnovaGraphics.Enums;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Implementations;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Services.Implementations
{
    public class BackgroundService : IBackgroundService
    {
        private readonly IRepository<ShopItem> _shopItemRepository;

        public BackgroundService(
            IRepository<ShopItem> shopItemRepository)
        {
            _shopItemRepository = shopItemRepository;
        }

        public async Task<IEnumerable<Background>> GetAllAsync()
        {
            var allItems = await _shopItemRepository.GetAllAsync();
            return allItems.OfType<Background>();
        }

        public async Task<Background?> GetByIdAsync(Guid id)
        {
            var item = await _shopItemRepository.GetByIdAsync(id);
            return item as Background;
        }

        public async Task<Response> CreateAsync(CreateShopItemDto dto)
        {
            var validation = Validate(dto);
            if (!validation.Success)
                return validation;

            // Створюємо аватар
            var background = new Background
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Price = dto.Price,
                PhotoPath = dto.PhotoPath
            };

            await _shopItemRepository.AddAsync(background);

            return new Response
            {
                Success = true,
                Message = "Background created successfully",
                Data = background
            };
        }

        public async Task<Response> AddAsync(Background background)
        {
            if (background == null)
            {
                return new Response { Success = false, Message = "Invalid background data" };
            }

            await _shopItemRepository.AddAsync(background);
            return new Response { Success = true, Message = "Background added successfully" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var entity = await _shopItemRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return new Response { Success = false, Message = "Background not found" };
            }

            await _shopItemRepository.DeleteAsync(id);
            return new Response { Success = true, Message = "Background deleted successfully" };
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Background> patchDoc)
        {
            var entity = await _shopItemRepository.GetByIdAsync(id) as Background;
            if (entity == null)
            {
                return new Response { Success = false, Message = "Background not found" };
            }

            patchDoc.ApplyTo(entity);
            await _shopItemRepository.UpdateAsync(entity);

            return new Response { Success = true, Message = "Background updated successfully" };
        }

        private Response Validate(CreateShopItemDto dto)
        {
            if (dto.Type != ShopItemType.Background)
            {
                return new Response { Success = false, Message = "Type of ShopItem must be 'Background'" };
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
