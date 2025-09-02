using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Mediator.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Mediator.Implementations
{
    // Створює питання та відповіді і повертає в TestService
    public class TestCreationMediator : IMediator
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        // Класи валідатори - щоб винести логіку валідації з TestService
        private readonly IValidator<CreateQuestionDto, Guid> _questionValidator;
        private readonly IValidator<CreateAnswerDto, Guid> _answerValidator;

        public TestCreationMediator(
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IValidator<CreateQuestionDto, Guid> questionValidator,
            IValidator<CreateAnswerDto, Guid> answerValidator)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _questionValidator = questionValidator;
            _answerValidator = answerValidator;
        }

        public async Task<Response> CreateQuestionAsync(CreateQuestionDto newQuestionDto, Guid testId)
        {
            return await ValidateAndCreateAsync(
                newQuestionDto,
                testId,
                _questionValidator,
                "Невалідні дані для створення питання.",
                (dto, id) => CreateQuestionInternalAsync(dto, id)
            );
        }

        public async Task<Response> CreateAnswerAsync(CreateAnswerDto newAnswerDto, Guid questionId)
        {
            return await ValidateAndCreateAsync(
                newAnswerDto,
                questionId,
                _answerValidator,
                "Невалідні дані для створення відповіді.",
                (dto, id) => CreateAnswerInternalAsync(dto, id)
            );
        }

        // Узагальнений метод для валідації та створення
        private async Task<Response> ValidateAndCreateAsync<TDto, TId, TEntity>(
            TDto dto,
            TId id,
            IValidator<TDto, TId> validator,
            string validationErrorMessage,
            Func<TDto, TId, Task<TEntity>> creationFunc)
            where TDto : class
            where TEntity : class
        {
            var validationErrors = await validator.ValidateAsync(dto, id);
            if (validationErrors.Any())
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = validationErrorMessage,
                    ValidationErrors = validationErrors
                };
            }

            var newEntity = await creationFunc(dto, id);
            return new Response
            {
                Success = true,
                StatusCode = 201,
                Message = $"{typeof(TEntity).Name} успішно створено!",
                Data = newEntity
            };
        }

        // Методи для власне створення об'єктів (без валідації та респонсів)
        private async Task<Question> CreateQuestionInternalAsync(CreateQuestionDto newQuestionDto, Guid testId)
        {
            var newQuestion = new Question
            {
                Id = Guid.NewGuid(),
                Text = newQuestionDto.Text,
                Number = newQuestionDto.Number,
                TestId = testId,
                Answers = new List<Answer>()
            };
            await _questionRepository.AddAsync(newQuestion);
            return newQuestion;
        }

        private async Task<Answer> CreateAnswerInternalAsync(CreateAnswerDto newAnswerDto, Guid questionId)
        {
            var newAnswer = new Answer
            {
                Id = Guid.NewGuid(),
                Text = newAnswerDto.Text,
                IsCorrect = newAnswerDto.IsCorrect,
                QuestionId = questionId
            };
            await _answerRepository.AddAsync(newAnswer);
            return newAnswer;
        }
    }
}