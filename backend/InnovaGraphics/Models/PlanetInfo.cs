namespace InnovaGraphics.Models
{
    public class PlanetInfo
    {
        public Guid Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int UsedCoinCount { get; set; }
        public int UsedEnergyCount { get; set; }
        public int UsedMarkCount { get; set; }

        //N:1
        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }
    }
}
