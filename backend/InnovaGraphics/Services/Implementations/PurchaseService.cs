using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ShopItem> _shopItemRepository;

        public PurchaseService(
            IRepository<Purchase> purchaseRepository,
            IRepository<User> userRepository,
            IRepository<ShopItem> shopItemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _shopItemRepository = shopItemRepository;
        }

        public async Task<IEnumerable<ReadPurchaseDto>> GetAllAsync()
        {
            var purchases = await _purchaseRepository.GetAllAsync();
            var result = new List<ReadPurchaseDto>();

            foreach (var p in purchases)
            {
                result.Add(new ReadPurchaseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User?.UserName ?? "(unknown)",
                    ShopItemId = p.ShopItemId,
                    ShopItemName = p.ShopItem?.Name ?? "(unknown)",
                    PurchaseDate = p.DateTime
                });
            }
            return result;
        }

        public async Task<ReadPurchaseDto?> GetByIdAsync(Guid id)
        {
            var p = await _purchaseRepository.GetByIdAsync(id);
            if (p == null) return null;

            return new ReadPurchaseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.User?.UserName ?? "(unknown)",
                ShopItemId = p.ShopItemId,
                ShopItemName = p.ShopItem?.Name ?? "(unknown)",
                PurchaseDate = p.DateTime
            };
        }

        public async Task<IEnumerable<ReadPurchaseDto>> GetByUserIdAsync(Guid userId)
        {
            var allPurchases = await _purchaseRepository.GetAllAsync();
            var userPurchases = allPurchases
                .Where(p => p.UserId == userId.ToString())
                .Select(p => new ReadPurchaseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User?.UserName ?? "(unknown)",
                    ShopItemId = p.ShopItemId,
                    ShopItemName = p.ShopItem?.Name ?? "(unknown)",
                    PurchaseDate = p.DateTime
                });

            return userPurchases;
        }


        public async Task<Response> BuyItemAsync(Guid userId, Guid shopItemId)
        {
            // Завантажуємо користувача
            var user = await _userRepository.GetByIdAsync(userId.ToString());
            if (user == null)
                return new Response { Success = false, Message = "User not found" };

            // Завантажуємо товар (ShopItem)
            var item = await _shopItemRepository.GetByIdAsync(shopItemId);
            if (item == null)
                return new Response { Success = false, Message = "Shop item not found" };

            // Перевіряємо, чи вже куплено
            var existingPurchase = (await _purchaseRepository.GetAllAsync())
                .FirstOrDefault(p => p.UserId == user.Id && p.ShopItemId == item.Id);
            if (existingPurchase != null)
                return new Response { Success = false, Message = "Item already purchased" };

            // Якщо ціна > 0, перевіряємо баланс користувача
            if (item.Price > 0 && user.CoinCount < item.Price)
                return new Response { Success = false, Message = "Not enough coins" };

            // Списуємо монети (якщо є ціна)
            if (item.Price > 0)
            {
                user.CoinCount -= (int)item.Price;
                await _userRepository.UpdateAsync(user);
            }
            Console.WriteLine(user.Profile);
            // Припускаємо, що ShopItem.Id є ідентифікатором аватара
            if (user.Profile != null)
            {
                user.Profile.AvatarId = item.Id;
            }

            await _userRepository.UpdateAsync(user);

            // Створюємо покупку
            var purchase = new Purchase
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ShopItemId = item.Id,
                DateTime = DateTime.UtcNow
            };

            await _purchaseRepository.AddAsync(purchase);

            return new Response { Success = true, Message = "Item purchased successfully", Data = purchase };
        }


        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Purchase> patchDoc)
        {
            if (patchDoc == null)
                return new Response { Success = false, Message = "Invalid patch document" };

            var existing = await _purchaseRepository.GetByIdAsync(id);
            if (existing == null)
                return new Response { Success = false, Message = "Purchase not found" };

            patchDoc.ApplyTo(existing);

            // Перевірки (за потреби):
            if (existing.UserId == null || existing.ShopItemId == Guid.Empty)
                return new Response { Success = false, Message = "User or ShopItem cannot be empty" };

            await _purchaseRepository.UpdateAsync(existing);

            return new Response { Success = true, Message = "Purchase updated successfully", Data = existing };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var existing = await _purchaseRepository.GetByIdAsync(id);
            if (existing == null)
                return new Response { Success = false, Message = "Purchase not found" };

            await _purchaseRepository.DeleteAsync(id);
            return new Response { Success = true, Message = "Purchase deleted" };
        }

        // Реалізація IBaseService<Purchase>.GetAllAsync
        async Task<IEnumerable<Purchase>> IBaseService<Purchase>.GetAllAsync()
        {
            return await _purchaseRepository.GetAllAsync();
        }

        // Реалізація IBaseService<Purchase>.GetByIdAsync
        async Task<Purchase> IBaseService<Purchase>.GetByIdAsync(Guid id)
        {
            return await _purchaseRepository.GetByIdAsync(id);
        }
    }
}
