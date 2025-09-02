namespace InnovaGraphics.Interactions
{
    public class CodeResponse
    {
        public string OutputText { get; set; }
        public List<double[]>? CurvePoints { get; set; }
        public bool Success { get; set; }
    }
}
