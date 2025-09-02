namespace InnovaGraphics.Models
{
    public class Case
    {
        public Guid Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        //N:1
        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }
}
