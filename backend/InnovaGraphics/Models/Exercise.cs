using InnovaGraphics.Enums;

namespace InnovaGraphics.Models
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Theme { get; set; }
        public bool IsLiked { get; set; }
        public LevelEnum DifficultyLevel { get; set; }
        public int Reward { get; set; }

        //N:1
        public Guid? PlanetId { get; set; }
        public Planet Planet { get; set; }

        //1:N
        public List<Case> Cases { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
