using InnovaGraphics.Enums;

namespace InnovaGraphics.Dtos
{
    public class MaterialResponseGeneral
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TypeEnum Type { get; set; }
        public string? Description { get; set; }
        public string Link { get; set; }
        public string PhotoPath { get; set; }
        public List<ThemeEnum> Theme { get; set; }
    }
}
