using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IPurchaseService : IBaseService<Purchase>
    {
        Task<Response> BuyItemAsync(Guid userId, Guid shopItemId);
        Task<IEnumerable<ReadPurchaseDto>> GetByUserIdAsync(Guid userId);
    }
}
