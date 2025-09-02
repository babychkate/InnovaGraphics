using InnovaGraphics.Structs;

namespace InnovaGraphics.Interactions
{
    public class CodeRequest5
    {
        public string SourceCode { get; set; }
        public string TaskType { get; set; }
        public Guid ExerciseId { get; set; }

        // Нове поле для початкових точок (X, Y)
        public List<double[]> InputPoints { get; set; } = new List<double[]>();
    }
}
