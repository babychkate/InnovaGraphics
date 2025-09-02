namespace InnovaGraphics.Dtos
{
    public class CreateHintDto
    {
        public Guid Id { get; set; }
        public Guid PlanetId { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
