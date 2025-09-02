namespace InnovaGraphics.Models
{
    public class Method
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string Description { get; set; }

        //N:1
        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }
    }
}
