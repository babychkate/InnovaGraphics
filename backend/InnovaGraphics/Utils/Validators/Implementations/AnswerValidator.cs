using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class AnswerValidator : IValidator<Answer, Guid>
    {
        private readonly IAnswerRepository _answerRepository;
        private const string TextFieldName = "text";
        private const string IsCorrectText = "isCorrect";
        private readonly int MinAnswerTextLength = 1;
        private readonly int MaxAnswerTextLength = 100;
        public AnswerValidator(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        public async Task<Dictionary<string, List<string>>> ValidateAsync(Answer answer, Guid id)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(answer.Text) || answer.Text.Length < MinAnswerTextLength || answer.Text.Length > MaxAnswerTextLength)
            {
                var message = string.IsNullOrEmpty(answer.Text)
                    ? "Текст відповіді не може бути порожнім."
                    : $"Текст відповіді має бути від {MinAnswerTextLength} до {MaxAnswerTextLength} символів.";
                AddValidationError(validationErrors, TextFieldName, message);
            }
            else
            {
                // Перевірка на унікальність тексту відповіді в межах поточного питання (виключаючи поточну відповідь)
                var existingAnswersWithSameText = await _answerRepository.GetAnswersByQuestionIdAsync(answer.QuestionId);
                if (existingAnswersWithSameText.Any(a => a.Id != id && a.Text.ToLower() == answer.Text.ToLower()))
                {
                    AddValidationError(validationErrors, TextFieldName, $"Відповідь з текстом '{answer.Text}' вже існує для цього питання.");
                }
            }

            // Перевірка на унікальність IsCorrect
            if (answer.IsCorrect)
            {
                var existingCorrectAnswer = await _answerRepository.GetAnswersByQuestionIdAsync(answer.QuestionId);
                if (existingCorrectAnswer.Any(a => a.Id != id && a.IsCorrect))
                {
                    AddValidationError(validationErrors, IsCorrectText, "Для цього питання вже існує правильна відповідь.");
                }
            }

            // Перевірка QuestionId
            if (answer.QuestionId == Guid.Empty)
            {
                AddValidationError(validationErrors, "questionId", "ID питання не може бути порожнім.");
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
