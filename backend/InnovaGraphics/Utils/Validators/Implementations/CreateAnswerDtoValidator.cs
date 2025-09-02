using InnovaGraphics.Dtos;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class CreateAnswerDtoValidator : IValidator<CreateAnswerDto, Guid>
    {
        private readonly IAnswerRepository _answerRepository;
        private const string TextFieldName = "text";
        private const string IsCorrectText = "isCorrect";
        private readonly int MinAnswerTextLength = 1;
        private readonly int MaxAnswerTextLength = 100;

        public CreateAnswerDtoValidator(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<Dictionary<string, List<string>>> ValidateAsync(CreateAnswerDto newAnswerDto, Guid questionId)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Перевірка тексту відповіді
            // Валідація Text
            if (string.IsNullOrEmpty(newAnswerDto.Text) || newAnswerDto.Text.Length < MinAnswerTextLength || newAnswerDto.Text.Length > MaxAnswerTextLength)
            {
                var message = string.IsNullOrEmpty(newAnswerDto.Text)
                    ? "Текст відповіді не може бути порожнім."
                    : $"Текст відповіді має бути від {MinAnswerTextLength} до {MaxAnswerTextLength} символів.";
                AddValidationError(validationErrors, TextFieldName, message);
            }
            else
            {
                // Перевірка на унікальність тексту відповіді в межах поточного питання
                var existingAnswersWithSameText = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
                if (existingAnswersWithSameText.Any(a => a.Text.ToLower() == newAnswerDto.Text.ToLower()))
                {
                    AddValidationError(validationErrors, TextFieldName, $"Відповідь з текстом '{newAnswerDto.Text}' вже існує для цього питання.");
                }
            }

            // Перевірка на унікальність IsCorrect
            if (newAnswerDto.IsCorrect)
            {
                var existingCorrectAnswer = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
                if (existingCorrectAnswer.Any(a => a.IsCorrect))
                {
                    AddValidationError(validationErrors, IsCorrectText, "Для цього питання вже існує правильна відповідь.");
                }
            }
            return validationErrors;
        }

        private void AddValidationError(Dictionary<string, List<string>> errors, string fieldName, string message)
        {
            if (!errors.ContainsKey(fieldName))
            {
                errors[fieldName] = new List<string>();
            }
            errors[fieldName].Add(message);
        }
    }
}
