namespace InnovaGraphics.Dtos
{
    public class ReadPurchaseDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Guid ShopItemId { get; set; }
        public string ShopItemName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
