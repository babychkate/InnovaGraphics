namespace InnovaGraphics.Models
{
    public class Resource : ShopItem
    {
        public string Subtype { get; set; }
        public int Value { get; set; }

        //1:1
        public Profile Profile { get; set; }
    }
}
