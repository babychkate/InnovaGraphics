using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExistenceChecker<Test, Guid> _testExistenceChecker;
        private readonly IValidator<CreateQuestionDto, Guid> _createQuestionValidator;
        private readonly IValidator<Question, Guid> _questionValidator;
        private readonly IValidator<Answer, Guid> _answerValidator;

        public QuestionService(
            IQuestionRepository questionRepository,
            IExistenceChecker<Test, Guid> testExistenceChecker,
            IValidator<CreateQuestionDto, Guid> createQuestionValidator,
            IValidator<Question, Guid> questionValidator,
            IValidator<Answer, Guid> answerValidator)
        {
            _questionRepository = questionRepository;
            _testExistenceChecker = testExistenceChecker;
            _createQuestionValidator = createQuestionValidator;
            _questionValidator = questionValidator;
            _answerValidator = answerValidator;
        }

        // READ
        public async Task<Question> GetQuestionByIdAsync(Guid id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByTestIdAsync(Guid testId)
        {
            if (testId == Guid.Empty)
            {
                return Enumerable.Empty<Question>();
            }
            return await _questionRepository.GetQuestionsByTestIdAsync(testId);
        }

        // CREATE
        public async Task<Response> CreateQuestionAsync(CreateQuestionDto newQuestionDto, Guid testId)
        {
            var validationErrors = await _createQuestionValidator.ValidateAsync(newQuestionDto, testId);

            if (validationErrors.Any())
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Невалідні дані для створення питання.",
                    ValidationErrors = validationErrors
                };
            }

            var newQuestion = new Question
            {
                Id = Guid.NewGuid(),
                Text = newQuestionDto.Text,
                Number = newQuestionDto.Number,
                TestId = testId,
                Answers = new List<Answer>()
            };

            await _questionRepository.AddAsync(newQuestion);
            return new Response
            {
                Success = true,
                StatusCode = 201,
                Message = "Запитання успішно створено!"
            };
        }


        // UPDATE
        public async Task<Response> UpdateQuestionAsync(Guid id, JsonPatchDocument<Question> patchDoc)
        {
            var questionToUpdate = await _questionRepository.GetByIdWithAnswersAsync(id);

            if (questionToUpdate == null)
            {
                return new Response { Success = false, StatusCode = 404, Message = $"Питання з ID '{id}' не знайдено." };
            }

            // Зберігаємо початковий стан відповідей для порівняння (якщо потрібно валідувати лише змінені)
            var originalAnswers = questionToUpdate.Answers?.ToList();

            patchDoc.ApplyTo(questionToUpdate);

            var questionValidationErrors = await _questionValidator.ValidateAsync(questionToUpdate, id);
            var allValidationErrors = questionValidationErrors;

            // Валідація кожної відповіді в колекції
            if (questionToUpdate.Answers != null)
            {
                foreach (var answer in questionToUpdate.Answers)
                {
                    var answerValidationResult = await _answerValidator.ValidateAsync(answer, answer.Id); // Передаємо ID відповіді для перевірки унікальності

                    if (answerValidationResult.Any())
                    {
                        // Об'єднуємо помилки валідації відповіді з загальними помилками
                        foreach (var error in answerValidationResult)
                        {
                            if (!allValidationErrors.ContainsKey(error.Key))
                            {
                                allValidationErrors[error.Key] = new List<string>();
                            }
                            allValidationErrors[error.Key].AddRange(error.Value);
                        }
                    }
                }
            }

            // Перевірка на наявність помилок валідації
            if (allValidationErrors.Any())
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Невалідні дані для оновлення питання.",
                    ValidationErrors = allValidationErrors
                };
            }

            // Перевірка існування тесту (як і раніше)
            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/testid"))
            {
                if (!await _testExistenceChecker.ExistsAsync(questionToUpdate.TestId))
                {
                    return new Response
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = $"Тест з ID '{questionToUpdate.TestId}' не знайдено."
                    };
                }
            }

            await _questionRepository.UpdateAsync(questionToUpdate);
            return new Response
            {
                Success = true,
                StatusCode = 204,
                Message = "Питання успішно оновлено"
            };
        }

        // DELETE
        public async Task<Response> DeleteQuestionAsync(Guid id)
        {
            var existingQuestion = await _questionRepository.GetByIdAsync(id);
            if (existingQuestion == null)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = $"Питання з ID '{id}' не знайдено."
                };
            }

            await _questionRepository.DeleteAsync(id);

            return new Response
            {
                Success = true,
                StatusCode = 200,
                Message = $"Питання з ID '{id}' успішно видалено."
            };
        }
    }
}