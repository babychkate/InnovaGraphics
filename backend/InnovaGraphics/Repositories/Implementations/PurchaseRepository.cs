using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ShopItem?> GetPurchasedShopItemAsync(string userId, Guid shopItemId)
        {
            return await _context.Purchases
                                 .Where(p => p.UserId == userId && p.ShopItemId == shopItemId)
                                 .Select(p => p.ShopItem) 
                                 .FirstOrDefaultAsync();
        }

        public async Task<bool> HasUserPurchasedItemAsync(string userId, Guid shopItemId)
        {
            return await _context.Purchases
                                 .AnyAsync(p => p.UserId == userId && p.ShopItemId == shopItemId);
        }
    }
}

