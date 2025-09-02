using InnovaGraphics.Models;

namespace InnovaGraphics.Interactions
{
    public class TestResult
    {
        public bool Success { get; set; }
        public List<string> ListErrors { get; set; } = new List<string>();
        public Dictionary<Guid, bool> CaseResults { get; set; } = new Dictionary<Guid, bool>();
    }
}
