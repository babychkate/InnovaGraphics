namespace InnovaGraphics.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
