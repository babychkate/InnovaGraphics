using InnovaGraphics.Enums;

namespace InnovaGraphics.Models
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TypeEnum Type { get; set; }
        public string? Description { get; set; }
        public string Link { get; set; }
        public string PhotoPath { get; set; }
        public List<ThemeEnum> Theme { get; set; }
        public bool IsLiked { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
