using InnovaGraphics.Enums;

namespace InnovaGraphics.Models
{
    public class ShopItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ShopItemType Type { get; set; }
        public decimal  Price { get; set; }
        public string PhotoPath { get; set; }

        //1:N
        public List<Purchase> Purchases { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
