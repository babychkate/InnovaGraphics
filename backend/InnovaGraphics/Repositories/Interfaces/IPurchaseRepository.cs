using InnovaGraphics.Models; 

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<bool> HasUserPurchasedItemAsync(string userId, Guid shopItemId);
        Task<ShopItem?> GetPurchasedShopItemAsync(string userId, Guid shopItemId);
    }
}
