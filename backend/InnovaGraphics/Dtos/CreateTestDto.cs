using System.ComponentModel.DataAnnotations;

namespace InnovaGraphics.Dtos
{
    public class CreateTestDto
    {
        public string Name { get; set; }

        public string PlanetName { get; set; }

        public string Theme { get; set; }

        public TimeOnly TimeLimit { get; set; }

        public bool IsQuickTest { get; set; }
    }
}
