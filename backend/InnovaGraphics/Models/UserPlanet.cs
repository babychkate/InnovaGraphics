namespace InnovaGraphics.Models
{
    public class UserPlanet
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public Guid PlanetId { get; set; }
        public Planet Planet { get; set; }
    }

}
