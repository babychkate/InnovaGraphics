namespace InnovaGraphics.Models
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }

        //N:1
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid ShopItemId { get; set; }
        public ShopItem ShopItem { get; set; }
    }
}
