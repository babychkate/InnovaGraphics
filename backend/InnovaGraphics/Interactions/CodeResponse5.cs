using InnovaGraphics.Structs;

namespace InnovaGraphics.Interactions
{
    public class CodeResponse5
    {
        public string OutputText { get; set; }

        // Початкові точки (які були передані)
        public List<double[]>? RealPoints { get; set; }

        // Очікувані/Результат трансформації
        public List<double[]>? ExpectedPoints { get; set; }
        public bool LooksAsExpected { get; set; }

        public bool Success { get; set; }
    }
}

