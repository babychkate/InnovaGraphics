using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class QuestionValidator : IValidator<Question, Guid>
    {
        private const string TextFieldName = "text";
        private const string NumberFieldName = "number";
        private const string TestIdFieldName = "testId";
        private const int MinQuestionTextLength = 5;
        private const int MaxQuestionTextLength = 500;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITestRepository _testRepository;
        public QuestionValidator(IQuestionRepository questionRepository, ITestRepository testRepository)
        {
            _questionRepository = questionRepository;
            _testRepository = testRepository;
        }
        public async Task<Dictionary<string, List<string>>> ValidateAsync(Question question, Guid id)
        {
            var validationErrors = new Dictionary<string, List<string>>();
            // Валідація Text
            if (string.IsNullOrEmpty(question.Text) || question.Text.Length < MinQuestionTextLength || question.Text.Length > MaxQuestionTextLength)
            {
                AddValidationError(validationErrors, TextFieldName, $"Текст питання має бути від {MinQuestionTextLength} до {MaxQuestionTextLength} символів.");
            }
            // Валідація Number
            if (question.Number <= 0)
            {
                AddValidationError(validationErrors, NumberFieldName, "Номер питання повинен бути більшим за 0.");
            }
            else
            {
                var existingQuestionWithNumber = await _questionRepository.GetQuestionByTestIdAndNumberAsync(question.TestId, question.Number);
                if (existingQuestionWithNumber != null && existingQuestionWithNumber.Id != id)
                {
                    AddValidationError(validationErrors, NumberFieldName, $"Питання з номером '{question.Number}' вже існує в цьому тесті.");
                }
            }

            // Валідація TestId
            if (question.TestId == Guid.Empty)
            {
                AddValidationError(validationErrors, TestIdFieldName, "ID тесту не може бути порожнім.");
            }
            else
            {
                var testExists = await _testRepository.GetByIdAsync(question.TestId);
                if (testExists == null)
                {
                    AddValidationError(validationErrors, TestIdFieldName, $"Тест з ID '{question.TestId}' не знайдено.");
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