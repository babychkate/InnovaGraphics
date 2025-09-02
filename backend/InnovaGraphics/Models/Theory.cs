namespace InnovaGraphics.Models
{
    public class Theory
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        //N:1
        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }
    }
}
