using InnovaGraphics.Enums;

namespace InnovaGraphics.Dtos
{
    public class CreateMaterialDto
    {
        public TypeEnum Type { get; set; }
        public List<ThemeEnum> Theme { get; set; }
        public string Link { get; set; }
    }
}
