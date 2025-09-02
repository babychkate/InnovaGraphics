using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class TestValidator : IValidator<Test, Guid>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExistenceChecker<Planet, Guid> _planetExistenceChecker;
        private const string NameFieldName = "name";
        private const string ThemeFieldName = "theme";
        private const string TimeLimitFieldName = "timeLimit";
        private const string PlanetFieldName = "planet";
        private const int MinNameLength = 2;
        private const int MaxNameLength = 30;
        private static readonly TimeOnly MinTimeLimit = new TimeOnly(0, 1, 0);
        private static readonly TimeOnly MaxTimeLimit = new TimeOnly(0, 15, 0);

        public TestValidator(ITestRepository testRepository, IExistenceChecker<Planet, Guid> planetExistenceChecker)
        {
            _testRepository = testRepository;
            _planetExistenceChecker = planetExistenceChecker;
        }

        public async Task<Dictionary<string, List<string>>> ValidateAsync(Test test, Guid id)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Валідація Name
            if (test.Name != null)
            {
                if (string.IsNullOrEmpty(test.Name) || test.Name.Length < MinNameLength || test.Name.Length > MaxNameLength)
                {
                    AddValidationError(validationErrors, NameFieldName, string.IsNullOrEmpty(test.Name) ? "Назва тесту не може бути порожнім." : $"Назва тесту має бути від {MinNameLength} до {MaxNameLength} символів.");
                }
                else
                {
                    var existingTest = await _testRepository.GetByNameAsync(test.Name);
                    if (existingTest != null && existingTest.Id != id)
                    {
                        AddValidationError(validationErrors, NameFieldName, $"Тест з назвою '{test.Name}' вже існує.");
                    }
                }
            }

            // Валідація Theme
            if (test.Theme != null)
            {
                if (string.IsNullOrEmpty(test.Theme))
                {
                    AddValidationError(validationErrors, ThemeFieldName, "Тема тесту не може бути порожнім.");
                }
            }

            // Валідація TimeLimit
            if (test.TimeLimit != default)
            {
                if (test.TimeLimit < MinTimeLimit || test.TimeLimit > MaxTimeLimit)
                {
                    AddValidationError(validationErrors, TimeLimitFieldName, test.TimeLimit < MinTimeLimit ? $"Лімітний час має бути не менше ніж {MinTimeLimit}." : $"Лімітний час має бути не більше ніж {MaxTimeLimit}.");
                }
            }

            // Валідація PlanetId (якщо змінюється через Planet.Name)
            if (test.Planet?.Name != null)
            {
                var planetExists = await _planetExistenceChecker.ExistsAsync(test.Planet.Name);
                if (!planetExists)
                {
                    AddValidationError(validationErrors, PlanetFieldName, $"Планети з назвою '{test.Planet.Name}' не існує.");
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