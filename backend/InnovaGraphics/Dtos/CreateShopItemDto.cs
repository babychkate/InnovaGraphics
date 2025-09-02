using InnovaGraphics.Enums;

namespace InnovaGraphics.Dtos
{
    public class CreateShopItemDto
    {
        public string Name { get; set; }
        public ShopItemType Type { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
    }
}
