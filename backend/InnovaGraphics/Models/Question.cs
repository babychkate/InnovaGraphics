namespace InnovaGraphics.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }

        //N:1
        public Guid TestId { get; set; }
        public Test Test { get; set; }

        //1:N
        public List<Answer> Answers { get; set; }
    }
}
