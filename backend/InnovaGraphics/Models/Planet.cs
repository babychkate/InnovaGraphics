namespace InnovaGraphics.Models
{
    public class Planet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string? SubTopic { get; set; }
        public string ImagePath { get; set; }
        public int RequiredEnergy { get; set; }
        public int EnergyLost { get; set; }
        public int Number { get; set; }
        public bool IsUnlock {  get; set; }
        public int MaxHintCount { get; set; }
        public int CurrentHintCount { get; set; }

        //1:1
        public PlanetAccess PlanetAccess { get; set; }

        //1:N
        public List<Theory> Theories { get; set; }
        public List<Test> Tests { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<PlanetInfo> PlanetInfos { get; set; } 
        public List<Method> Methods { get; set; }
        public List<Hint> Hints { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
