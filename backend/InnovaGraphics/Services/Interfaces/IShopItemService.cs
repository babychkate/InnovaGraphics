using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IShopItemService : IBaseService<ShopItem>
    {
        Task<Response> CreateAsync(CreateShopItemDto itemDto);
    }
}
