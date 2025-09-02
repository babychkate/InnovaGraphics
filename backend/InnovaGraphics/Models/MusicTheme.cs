namespace InnovaGraphics.Models
{
    public class MusicTheme : ShopItem
    {
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public Profile Profile { get; set; }
    }
}
