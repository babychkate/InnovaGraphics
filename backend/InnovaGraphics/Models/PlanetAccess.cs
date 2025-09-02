namespace InnovaGraphics.Models
{
    public class PlanetAccess : ShopItem
    {
        //1:1
        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }
    }
}
