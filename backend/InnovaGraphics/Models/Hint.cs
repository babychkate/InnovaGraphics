namespace InnovaGraphics.Models
{
    public class Hint : ShopItem
    {
        public int Number { get; set; }
        public string Text { get; set; }

        //N:1
        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
