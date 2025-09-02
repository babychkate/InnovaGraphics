using System.ComponentModel.DataAnnotations;

namespace InnovaGraphics.Dtos
{
    public class CreateQuestionWithAnswersDto
    {
        public string Text { get; set; }

        public int Number { get; set; }

        public List<CreateAnswerDto> Answers { get; set; }
    }
}
