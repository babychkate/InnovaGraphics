using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;

namespace InnovaGraphics.Repositories.Implementations
{
    public class ShopItemRepository : BaseRepository<ShopItem>, IShopItemRepository
    {
        public ShopItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
