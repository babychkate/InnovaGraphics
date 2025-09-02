using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class ShopItemService : IShopItemService
    {
        private readonly IRepository<ShopItem> _shopItemRepository;

        public ShopItemService(IRepository<ShopItem> shopItemRepository)
        {
            _shopItemRepository = shopItemRepository;
        }

        public async Task<IEnumerable<ShopItem>> GetAllAsync()
        {
            return await _shopItemRepository.GetAllAsync();
        }

        // Тут буде передаватися id конкретної покупки, а вже потім братися ShopItem
        public async Task<ShopItem> GetByIdAsync(Guid id)
        {
            return await _shopItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ShopItem>> GetByTypeAsync(Type type)
        {
            var allItems = await _shopItemRepository.GetAllAsync();
            return allItems.Where(i => i.GetType() == type);
        }

        public async Task<Response> AddAsync(ShopItem item)
        {
            if (item == null)
                return new Response { Success = false, Message = "Invalid shop item data" };

            await _shopItemRepository.AddAsync(item);
            return new Response { Success = true, Message = "Shop item added successfully" };
        }

        public async Task<Response> CreateAsync(CreateShopItemDto itemDto)
        {
            if (itemDto == null)
                return new Response { Success = false, Message = "Invalid shop item data" };

            if (string.IsNullOrWhiteSpace(itemDto.Name))
                return new Response { Success = false, Message = "Name cannot be empty" };

            if (itemDto.Price <= 0)
                return new Response { Success = false, Message = "Price must be positive" };

            var newItem = new ShopItem
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Type = itemDto.Type,
                Price = itemDto.Price,
                PhotoPath = itemDto.PhotoPath,
            };

            await _shopItemRepository.AddAsync(newItem);

            return new Response { Success = true, Message = "Shop item created successfully", Data = newItem };
        }


        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<ShopItem> patchDoc)
        {
            if (patchDoc == null)
                return new Response { Success = false, Message = "Invalid patch document" };

            var entity = await _shopItemRepository.GetByIdAsync(id);
            if (entity == null)
                return new Response { Success = false, Message = "Shop item not found" };

            patchDoc.ApplyTo(entity);

            if (string.IsNullOrWhiteSpace(entity.Name))
                return new Response { Success = false, Message = "Name cannot be empty" };

            if (entity.Price <= 0)
                return new Response { Success = false, Message = "Price must be positive" };

            if (string.IsNullOrWhiteSpace(entity.PhotoPath))
                return new Response { Success = false, Message = "Photo cannot be empty" };

            await _shopItemRepository.UpdateAsync(entity);

            return new Response { Success = true, Message = "Shop item updated successfully" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var entity = await _shopItemRepository.GetByIdAsync(id);
            if (entity == null)
                return new Response { Success = false, Message = "Shop item not found" };

            await _shopItemRepository.DeleteAsync(id);
            return new Response { Success = true, Message = "Shop item deleted successfully" };
        }

    }
}