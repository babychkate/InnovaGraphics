using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IBackgroundService : IBaseService<Background>
    {
        Task<Response> CreateAsync(CreateShopItemDto dto);
    }
}
