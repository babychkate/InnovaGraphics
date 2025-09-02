using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Mediator.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;

namespace InnovaGraphics.Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly IPlanetRepository _planetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IExistenceChecker<Planet, Guid> _planetExistenceChecker;
        private readonly IValidator<CreateTestDto, object> _createTestDtoValidator;
        private readonly IValidator<Test, Guid> _testValidator;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestService(
            ITestRepository testRepository,
            IMediator mediator,
            IExistenceChecker<Planet, Guid> planetExistenceChecker,
            IValidator<CreateTestDto, object> createTestDtoValidator,
            IValidator<Test, Guid> testValidator,
            UserManager<User> userManager,
            IPlanetRepository planetRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _testRepository = testRepository;
            _mediator = mediator;
            _planetExistenceChecker = planetExistenceChecker;
            _createTestDtoValidator = createTestDtoValidator;
            _testValidator = testValidator;
            _userManager = userManager;
            _planetRepository = planetRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Стандартні методи

        public async Task<Test> GetByIdAsync(Guid id) => await _testRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Test>> GetAllAsync() => await _testRepository.GetAllAsync();
        public async Task AddAsync(Test test) => await _testRepository.AddAsync(test);
        public async Task UpdateAsync(Test entity) => await _testRepository.UpdateAsync(entity);
        public async Task DeleteAsync(Guid id) => await _testRepository.DeleteAsync(id);

        // CREATE
        public async Task<TestResponseGeneralDto> CreateTestAsync(CreateTestDto newTestDto)
        {
            var validationErrors = await _createTestDtoValidator.ValidateAsync(newTestDto, null);

            if (validationErrors.Any())
            {
                return new TestResponseGeneralDto
                {
                    Message = "Невалідні дані для створення тесту.",
                    ValidationErrors = validationErrors
                };
            }

            Guid? planetId = null;
            if (!string.IsNullOrEmpty(newTestDto.PlanetName))
            {
                var planet = await _planetRepository.GetByNameAsync(newTestDto.PlanetName);
                if (planet != null)
                {
                    planetId = planet.Id;
                }
                else
                {
                    validationErrors.Add("planetName", new List<string> { $"Планети з назвою '{newTestDto.PlanetName}' не знайдено." });
                    return new TestResponseGeneralDto
                    {
                        Message = "Невалідні дані для створення тесту.",
                        ValidationErrors = validationErrors
                    };
                }
            }

                var newTest = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = newTestDto.Name,
                    Theme = newTestDto.Theme,
                    PlanetId = planetId, // Встановлюємо Id планети
                    IsQuickTest = newTestDto.IsQuickTest,
                    TimeLimit = newTestDto.TimeLimit,
                    TestResult = 0,
                    IsCompleted = false,
                    Questions = new List<Question>(),
                    Battles = new List<Battle>(),
                    Users = new List<User>(),
                };

            await _testRepository.AddAsync(newTest);
            return new TestResponseGeneralDto
            {
                Message = "Тест успішно створено!",
                Id = newTest.Id,
                Name = newTest.Name,
                Theme = newTest.Theme,
                TimeLimit = newTest.TimeLimit,
                TestResult = newTest.TestResult,
                IsQuickTest = newTest.IsQuickTest,
                IsCompleted = newTest.IsCompleted,
                PlanetId = newTest.PlanetId // Повертаємо встановлений PlanetId
            };
        }

        public async Task<TestResponseGeneralDto> AddQuestionsWithAnswersToTestAsync(List<CreateQuestionWithAnswersDto> newQuestionsWithAnswersList, Guid testId)
        {
            var existingTest = await _testRepository.GetByIdAsync(testId);
            if (existingTest == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{testId}' не знайдено." };
            }

            var createdQuestions = new List<QuestionGeneralDto>();

            foreach (var newQuestionWithAnswersDto in newQuestionsWithAnswersList)
            {
                var createQuestionDto = new CreateQuestionDto
                {
                    Text = newQuestionWithAnswersDto.Text,
                    Number = newQuestionWithAnswersDto.Number,
                };

                var questionResponse = await _mediator.CreateQuestionAsync(createQuestionDto, testId);
                if (questionResponse.ValidationErrors?.Any() == true)
                {
                    return new TestResponseGeneralDto
                    {
                        Message = questionResponse.Message,
                        ValidationErrors = questionResponse.ValidationErrors
                    };
                }
                if (questionResponse.Data == null && !string.IsNullOrEmpty(questionResponse.Message))
                {
                    return new TestResponseGeneralDto
                    {
                        Message = questionResponse.Message
                    };
                }

                var createdQuestion = (Question)questionResponse.Data!;
                var answers = new List<AnswerGeneralDto>();

                if (newQuestionWithAnswersDto.Answers != null && newQuestionWithAnswersDto.Answers.Any())
                {
                    foreach (var answerDto in newQuestionWithAnswersDto.Answers)
                    {
                        var answerResponse = await _mediator.CreateAnswerAsync(answerDto, createdQuestion.Id);
                        if (answerResponse.ValidationErrors?.Any() == true)
                        {
                            return new TestResponseGeneralDto
                            {
                                Message = "Помилка створення відповіді.",
                                ValidationErrors = answerResponse.ValidationErrors
                            };
                        }
                        if (answerResponse.Data == null && !string.IsNullOrEmpty(answerResponse.Message))
                        {
                            return new TestResponseGeneralDto
                            {
                                Message = "Помилка створення відповіді."
                            };
                        }
                        var createdAnswer = (Answer)answerResponse.Data!;
                        answers.Add(new AnswerGeneralDto
                        {
                            Id = createdAnswer.Id,
                            Text = createdAnswer.Text,
                            IsCorrect = createdAnswer.IsCorrect
                        });
                    }
                }

                createdQuestions.Add(new QuestionGeneralDto
                {
                    Id = createdQuestion.Id,
                    Text = createdQuestion.Text,
                    Number = createdQuestion.Number,
                    Answers = answers
                });
            }

            return new TestResponseGeneralDto
            {
                Message = "Питання та відповіді успішно додано!",
                Id = testId,
                Questions = createdQuestions
            };
        }

        // READ
        public async Task<TestResponseGeneralDto?> GetTestByIdWithQuestionsAsync(Guid id)
        {
            var testWithQuestions = await _testRepository.GetByIdWithQuestionsAsync(id);
            if (testWithQuestions == null)
            {
                return null; // Або можна повернути TestResponseGeneralDto з Message "Тест не знайдено"
            }

            return MapToTestResponseGeneralDto(testWithQuestions, includeQuestions: true);
        }

        public async Task<Test?> GetTestByIdWithQuestionsAndAnswersAsync(Guid id)
        {
            return await _testRepository.GetByIdWithQuestionsAndAnswersAsync(id);
        }

        private TestResponseGeneralDto MapToTestResponseGeneralDto(Test test, bool includeQuestions = false, bool includeQuestionsAndAnswers = false)
        {
            var dto = new TestResponseGeneralDto
            {
                Id = test.Id,
                Name = test.Name,
                Theme = test.Theme,
                TimeLimit = test.TimeLimit,
                TestResult = test.TestResult,
                IsQuickTest = test.IsQuickTest,
                IsCompleted = test.IsCompleted,
                PlanetId = test.PlanetId
            };

            if (includeQuestions && test.Questions != null)
            {
                dto.Questions = test.Questions.Select(q => new QuestionGeneralDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Number = q.Number,
                    // Answers залишаємо null, якщо не потрібно
                }).ToList();
            }

            if (includeQuestionsAndAnswers && test.Questions != null)
            {
                dto.Questions = test.Questions.Select(q => new QuestionGeneralDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Number = q.Number,
                    Answers = q.Answers?.Select(a => new AnswerGeneralDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList();
            }

            return dto;
        }

        // UPDATE
        public async Task<TestResponseGeneralDto> UpdateTestAsync(Guid id, JsonPatchDocument<Test> patchDoc)
        {
            var testToUpdate = await _testRepository.GetByIdAsync(id);
            if (testToUpdate == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{id}' не знайдено." };
            }

            patchDoc.ApplyTo(testToUpdate);

            // Отримуємо планету за назвою та оновлюємо PlanetId
            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/planet/name"))
            {
                if (!string.IsNullOrEmpty(testToUpdate.Planet?.Name))
                {
                    var planetExists = await _planetExistenceChecker.ExistsAsync(testToUpdate.Planet.Name);
                    if (!planetExists)
                    {
                        return new TestResponseGeneralDto { Message = $"Планети з назвою '{testToUpdate.Planet.Name}' не існує." };
                    }
                    testToUpdate.PlanetId = null;
                    testToUpdate.Planet = null;
                }
                else
                {
                    testToUpdate.PlanetId = null; // Дозволяємо скидати PlanetId
                    testToUpdate.Planet = null;
                }
            }
            // Якщо назва планети не була змінена, PlanetId залишається без змін

            var validationErrors = await _testValidator.ValidateAsync(testToUpdate, id);
            if (validationErrors.Any())
            {
                return new TestResponseGeneralDto
                {
                    Message = "Невалідні дані для оновлення тесту.",
                    ValidationErrors = validationErrors
                };
            }

            try
            {
                await _testRepository.UpdateAsync(testToUpdate);
                return new TestResponseGeneralDto
                {
                    Message = "Тест успішно оновлено!",
                    Id = testToUpdate.Id,
                    Name = testToUpdate.Name,
                    Theme = testToUpdate.Theme,
                    TimeLimit = testToUpdate.TimeLimit,
                    TestResult = testToUpdate.TestResult,
                    IsQuickTest = testToUpdate.IsQuickTest,
                    IsCompleted = testToUpdate.IsCompleted,
                    PlanetId = testToUpdate.PlanetId
                };
            }
            catch (Exception)
            {
                return new TestResponseGeneralDto { Message = "Помилка при оновленні тесту." };
            }
        }

        // DELETE
        public async Task<TestResponseGeneralDto> DeleteTest(Guid id)
        {
            var existingTest = await _testRepository.GetByIdAsync(id);
            if (existingTest == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{id}' не знайдено." };
            }

            try
            {
                await _testRepository.DeleteAsync(id);
                return new TestResponseGeneralDto
                {
                    Message = $"Тест з ID '{id}' успішно видалено.",
                    Id = existingTest.Id,
                    Name = existingTest.Name,
                    Theme = existingTest.Theme,
                    TimeLimit = existingTest.TimeLimit,
                    TestResult = existingTest.TestResult,
                    IsQuickTest = existingTest.IsQuickTest,
                    IsCompleted = existingTest.IsCompleted,
                    PlanetId = existingTest.PlanetId
                };
            }
            catch (Exception)
            {
                return new TestResponseGeneralDto { Message = "Помилка при видаленні тесту." };
            }
        }

        // ПОЧАТОК ТЕСТУ
        public async Task<TestResponseGeneralDto> StartTestAsync(string userEmail, Guid testId)
        {
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            if (identityUser == null)
            {
                return new TestResponseGeneralDto { Message = $"Користувача з поштою '{userEmail}' не знайдено." };
            }
            string userId = identityUser.Id;

            var test = await _testRepository.GetByIdWithUsersAsync(testId);

            if (test == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{testId}' не знайдено." };
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new TestResponseGeneralDto { Message = $"Користувача з ID '{userId}' не знайдено." };
            }

            if (test.PlanetId.HasValue)
            {
                var planet = await _planetRepository.GetByIdAsync(test.PlanetId.Value);
                if (planet == null || !planet.IsUnlock)
                {
                    return new TestResponseGeneralDto { Message = $"Планета '{(planet?.Name ?? "невідома")}' не розблокована для проходження цього тесту." };
                }

                // Перевірка енергії та її списання для планетарних тестів
                if (user.EnergyCount >= planet.RequiredEnergy)
                {
                    user.EnergyCount -= planet.EnergyLost;
                    await _userRepository.UpdateAsync(user);
                }
                else
                {
                    return new TestResponseGeneralDto { Message = "Недостатньо енергії для проходження тесту на цій планеті." };
                }
            }

            if (test.Users != null && test.Users.Any(u => u.Id == user.Id))
            {
                return new TestResponseGeneralDto
                {
                    Message = "Ви вже проходили цей тест.",
                    Id = testId,
                    IsCompleted = test.PlanetId != null
                };
            }

            if (test.Users == null)
            {
                test.Users = new List<User>();
            }

            test.Users.Add(user);
            await _testRepository.UpdateAsync(test);

            DateTime startTime = DateTime.UtcNow;
            string key = $"StartTime-{userId}-{testId}";
            _httpContextAccessor.HttpContext?.Session.SetString(key, JsonSerializer.Serialize(startTime));

            return new TestResponseGeneralDto
            {
                Message = "Тест успішно розпочато.",
                Id = testId,
                AdditionalData = new TestStartAdditionalDataDto { StartTime = startTime, TimeLimit = test.TimeLimit }
            };
        }
        public async Task<TestResponseGeneralDto> CompleteTestAsync(string userEmail, Guid testId, Dictionary<Guid, Guid?> userAnswers, string endTime)
        {
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            if (identityUser == null)
            {
                return new TestResponseGeneralDto { Message = $"Користувача з поштою '{userEmail}' не знайдено." };
            }
            string userId = identityUser.Id;

            var test = await GetTestByIdWithQuestionsAndAnswersAsync(testId);
            if (test == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{testId}' не знайдено." };
            }

            // Перевірка, чи користувач вже пов'язаний з цим тестом
            if (!test.Users.Any(u => u.Id == userId))
            {
                return new TestResponseGeneralDto { Message = $"Користувач з ID '{userId}' не знайдений серед тих, хто проходить тест з ID '{testId}'." };
            }
            string key = $"StartTime-{userId}-{testId}";
            string? startTimeJson = _httpContextAccessor.HttpContext?.Session.GetString(key);

            if (string.IsNullOrEmpty(startTimeJson))
            {
                return new TestResponseGeneralDto { Message = "Не знайдено час початку тесту." };
            }

            if (!DateTime.TryParse(endTime, out DateTime endTimeUtc))
            {
                return new TestResponseGeneralDto { Message = "Невалідний формат часу завершення." };
            }
            endTimeUtc = endTimeUtc.ToUniversalTime(); // Конвертуємо отриманий час до UTC

            DateTime startTimeUtc = JsonSerializer.Deserialize<DateTime>(startTimeJson);
            TimeSpan timeElapsed = endTimeUtc - startTimeUtc;
            TimeSpan allowedTime = test.TimeLimit.ToTimeSpan();

            var scoreResult = await CalculateTestScoreAsync(testId, userAnswers);

            if (scoreResult == null || scoreResult.ValidationErrors?.Any() == true)
            {
                return new TestResponseGeneralDto
                {
                    Message = scoreResult?.Message ?? "Помилка при розрахунку результатів тесту.",
                    ValidationErrors = scoreResult?.ValidationErrors
                };
            }

            int correctAnswersCount = 0;
            int incorrectAnswersCount = 0;
            int unansweredQuestionsCount = 0;
            var userAnswersWithCorrectness = new List<object>();

            foreach (var question in test.Questions)
            {
                Guid? userAnswerId = userAnswers.ContainsKey(question.Id) ? userAnswers[question.Id] : null;
                bool isCorrect = userAnswerId.HasValue && question.Answers.Any(a => a.Id == userAnswerId && a.IsCorrect);

                userAnswersWithCorrectness.Add(new
                {
                    QuestionId = question.Id,
                    UserAnswerId = userAnswerId,
                    IsCorrect = isCorrect
                });

                if (userAnswerId.HasValue)
                {
                    if (isCorrect)
                    {
                        correctAnswersCount++;
                    }
                    else
                    {
                        incorrectAnswersCount++;
                    }
                }
                else
                {
                    unansweredQuestionsCount++;
                }
            }

            int bonusPoints = 0;
            double timeDifferenceRatio = 1 - (timeElapsed.TotalSeconds / allowedTime.TotalSeconds);
            bonusPoints = (int)(timeDifferenceRatio * 50);
            if (bonusPoints < 0) bonusPoints = 0;


            var testResultDetails = new TestResultDto
            {
                Score = test.Questions.Count > 0 ? Convert.ToInt32(scoreResult.TestResult) : 0,
                Coins = Convert.ToInt32(scoreResult.TestResult) * 10 + bonusPoints,
                BonusPoints = bonusPoints,
                CorrectAnswers = correctAnswersCount,
                IncorrectAnswers = incorrectAnswersCount,
                Unanswered = unansweredQuestionsCount
            };

            var response = new TestResponseGeneralDto
            {
                Message = "Тест завершено.",
                Id = testId,
                TestResultDetails = testResultDetails,
                AdditionalData = userAnswersWithCorrectness
            };

            if (timeElapsed > allowedTime)
            {
                response.Message = "Тест завершено. Час вичерпано.";

            }

            return response;
        }
        public async Task<TestResponseGeneralDto> CalculateTestScoreAsync(Guid testId, Dictionary<Guid, Guid?> userAnswers)
        {
            var test = await GetTestByIdWithQuestionsAndAnswersAsync(testId);
            if (test == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест з ID '{testId}' не знайдено." };
            }

            int score = 0;
            foreach (var question in test.Questions)
            {
                if (userAnswers.ContainsKey(question.Id) && userAnswers[question.Id].HasValue)
                {
                    var userAnswerId = userAnswers[question.Id].Value;
                    if (question.Answers.Any(a => a.Id == userAnswerId && a.IsCorrect))
                    {
                        score++;
                    }
                }
            }

            return new TestResponseGeneralDto { TestResult = score };
        }

        public async Task<TestResponseGeneralDto?> GetTestBySubTopicAsync(string subTopic)
        {
            var test = await _testRepository.GetBySubTopicAsync(subTopic);

            if (test == null)
            {
                // If no test is found, return null or a DTO indicating no test found
                return new TestResponseGeneralDto { Message = $"Тест за підтемою '{subTopic}' не знайдено." };
            }

            // Map the found Test model to TestResponseGeneralDto for the frontend
            return MapToTestResponseGeneralDto(test);
        }

        public async Task<TestResponseGeneralDto> GetTestByPlanetIdAsync(Guid planetId)
        {
            var test = await _testRepository.GetByPlanetIdAsync(planetId);

            if (test == null)
            {
                return new TestResponseGeneralDto { Message = $"Тест для планети з ID '{planetId}' не знайдено." };
            }

            return MapToTestResponseGeneralDto(test);
        }

        // COLLECTION GETTERS
        public async Task<IEnumerable<string>> GetAllTestNamesAsync()
        {
            return await _testRepository.GetAllTestNamesAsync();
        }
    }
}