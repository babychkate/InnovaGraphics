using InnovaGraphics.Models;

namespace InnovaGraphics.Dtos
{
    public class CreatePlanetDto
    {
        public string Name { get; set; }
        public string Topic { get; set; }
        public string? SubTopic { get; set; }
        public string ImagePath { get; set; }
        public int RequiredEnergy { get; set; }
        public int EnergyLost { get; set; }
        public int Number { get; set; }
        public int MaxHintCount { get; set; }
    }
}
