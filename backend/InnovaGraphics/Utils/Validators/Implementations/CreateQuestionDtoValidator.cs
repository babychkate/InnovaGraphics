using InnovaGraphics.Dtos;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class CreateQuestionDtoValidator : IValidator<CreateQuestionDto, Guid>
    {
        private readonly IQuestionRepository _questionRepository;
        private const string TextFieldName = "text";
        private const string NumberText = "number";
        private readonly int MinQuestionTextLength = 5;
        private readonly int MaxQuestionTextLength = 200;

        public CreateQuestionDtoValidator(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Dictionary<string, List<string>>> ValidateAsync(CreateQuestionDto newQuestionDto, Guid testId)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Валідація Text
            if (string.IsNullOrEmpty(newQuestionDto.Text) || newQuestionDto.Text.Length < MinQuestionTextLength || newQuestionDto.Text.Length > MaxQuestionTextLength)
            {
                AddValidationError(validationErrors, TextFieldName, $"Текст питання має бути від {MinQuestionTextLength} до {MaxQuestionTextLength} символів.");

            }

            // Валідація Number
            if (newQuestionDto.Number <= 0)
            {
                AddValidationError(validationErrors, NumberText, "Номер питання повинен бути більшим за 0.");
            }
            else
            {
                // Перевірка на унікальність номера в межах поточного тесту
                var existingQuestionWithNumber = await _questionRepository.GetQuestionByTestIdAndNumberAsync(testId, newQuestionDto.Number);
                if (existingQuestionWithNumber != null)
                {
                    AddValidationError(validationErrors, NumberText, $"Питання з номером '{newQuestionDto.Number}' вже існує в цьому тесті.");
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
