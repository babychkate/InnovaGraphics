namespace InnovaGraphics.Dtos
{
    public class QuestionGeneralDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }
        public ICollection<AnswerGeneralDto>? Answers { get; set; }
    }
}
