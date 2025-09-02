 namespace InnovaGraphics.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        //N:1
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
