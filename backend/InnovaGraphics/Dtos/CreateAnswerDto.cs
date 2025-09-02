using InnovaGraphics.Models;

namespace InnovaGraphics.Dtos
{
    public class CreateAnswerDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
