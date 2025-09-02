using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class CreateTestDtoValidator : IValidator<CreateTestDto, object>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExistenceChecker<Planet, Guid> _planetExistenceChecker;
        private const string NameFieldName = "testname";
        private const string PlanetFieldName = "planet";
        private const string ThemeFieldName = "theme";
        private const string TimeLimitFieldName = "timelimit";
        private const int MinNameLength = 2;
        private const int MaxNameLength = 30;
        private static readonly TimeOnly MinTimeLimit = new TimeOnly(0, 1, 0);
        private static readonly TimeOnly MaxTimeLimit = new TimeOnly(0, 15, 0);

        public CreateTestDtoValidator(ITestRepository testRepository, IExistenceChecker<Planet, Guid> planetExistenceChecker)
        {
            _testRepository = testRepository;
            _planetExistenceChecker = planetExistenceChecker;
        }

        public async Task<Dictionary<string, List<string>>> ValidateAsync(CreateTestDto newTestDto, object _)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Валідація Name
            if (string.IsNullOrEmpty(newTestDto.Name) || newTestDto.Name.Length < MinNameLength || newTestDto.Name.Length > MaxNameLength)
            {
                AddValidationError(validationErrors, NameFieldName, string.IsNullOrEmpty(newTestDto.Name) ? "Назва тесту не може бути порожнім полем." : $"Назва тесту має бути від {MinNameLength} до {MaxNameLength} символів.");
            }
            else
            {
                var existingTest = await _testRepository.GetByNameAsync(newTestDto.Name);
                if (existingTest != null)
                {
                    AddValidationError(validationErrors, NameFieldName, $"Тест з назвою '{newTestDto.Name}' вже існує.");
                }
            }

            // Валідація PlanetName
            if (!string.IsNullOrEmpty(newTestDto.PlanetName))
            {
                var planetExists = await _planetExistenceChecker.ExistsAsync(newTestDto.PlanetName);
                if (!planetExists)
                {
                    AddValidationError(validationErrors, PlanetFieldName, $"Планети з назвою '{newTestDto.PlanetName}' не існує.");
                }
            }

            // Валідація Theme
            if (string.IsNullOrEmpty(newTestDto.Theme))
            {
                AddValidationError(validationErrors, ThemeFieldName, "Тема тесту не може бути порожнім полем.");
            }

            // Валідація TimeLimit
            if (newTestDto.TimeLimit == default || newTestDto.TimeLimit < MinTimeLimit || newTestDto.TimeLimit > MaxTimeLimit)
            {
                AddValidationError(validationErrors, TimeLimitFieldName, newTestDto.TimeLimit < MinTimeLimit ? $"Лімітний час має бути не менше ніж {MinTimeLimit}." : $"Лімітний час має бути не більше ніж {MaxTimeLimit}.");
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