using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IExistenceChecker<Question, Guid> _questionExistenceChecker;
        private readonly IValidator<CreateAnswerDto, Guid> _createAnswerValidator;
        private readonly IValidator<Answer, Guid> _answerValidator;
        private const int MinAnswerTextLength = 1;
        private const int MaxAnswerTextLength = 250;

        public AnswerService(
            IAnswerRepository answerRepository,
            IExistenceChecker<Question, Guid> questionExistenceChecker,
            IValidator<CreateAnswerDto, Guid> createAnswerValidator,
            IValidator<Answer, Guid> answerValidator)
        {
            _answerRepository = answerRepository;
            _questionExistenceChecker = questionExistenceChecker;
            _createAnswerValidator = createAnswerValidator;
            _answerValidator = answerValidator;
        }

        public async Task<Answer> GetAnswerByIdAsync(Guid id)
        {
            return await _answerRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            return await _answerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId)
        {
            if (questionId == Guid.Empty)
            {
                return Enumerable.Empty<Answer>();
            }
            return await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
        }

        // CREATE
        public async Task<Response> CreateAnswerAsync(Guid questionId, CreateAnswerDto newAnswerDto)
        {
            if (!await _questionExistenceChecker.ExistsAsync(questionId))
            {
                return new Response { 
                    Success = false, 
                    StatusCode = 404, 
                    Message = $"Питання з ID '{questionId}' не знайдено." };
            }

            var validationErrors = await _createAnswerValidator.ValidateAsync(newAnswerDto, questionId);
            if (validationErrors.Any())
            {
                return new Response { 
                    Success = false, 
                    StatusCode = 400, 
                    Message = "Невалідні дані для створення відповіді.", 
                    ValidationErrors = validationErrors };
            }

            var newAnswer = new Answer
            {
                Id = Guid.NewGuid(),
                Text = newAnswerDto.Text,
                IsCorrect = newAnswerDto.IsCorrect,
                QuestionId = questionId
            };

            await _answerRepository.AddAsync(newAnswer);
            return new Response { 
                Success = true, 
                StatusCode = 201, 
                Message = "Відповідь успішно створено!"};
        }

        // UPDATE

        public async Task<Response> UpdateAnswerAsync(Guid id, JsonPatchDocument<Answer> patchDoc)
        {
            var answerToUpdate = await _answerRepository.GetByIdAsync(id);
            if (answerToUpdate == null)
            {
                return new Response { 
                    Success = false, 
                    StatusCode = 404, 
                    Message = $"Відповідь з ID '{id}' не знайдено." };
            }

            patchDoc.ApplyTo(answerToUpdate);

            var validationErrors = await _answerValidator.ValidateAsync(answerToUpdate, id);
            if (validationErrors.Any())
            {
                return new Response { 
                    Success = false, 
                    StatusCode = 400, 
                    Message = "Невалідні дані для оновлення відповіді.", 
                    ValidationErrors = validationErrors };
            }

            await _answerRepository.UpdateAsync(answerToUpdate);
            return new Response { 
                Success = true, 
                StatusCode = 204, 
                Message = "Відповідь успішно оновлено." };
        }

        // DELETE
        public async Task<Response> DeleteAnswerAsync(Guid id)
        {
            var existingAnswer = await _answerRepository.GetByIdAsync(id);
            if (existingAnswer == null)
            {
                return new Response { 
                    Success = false, 
                    StatusCode = 404, 
                    Message = $"Відповідь з ID '{id}' не знайдено." };
            }

            await _answerRepository.DeleteAsync(id);
            return new Response { 
                Success = true, 
                StatusCode = 200, 
                Message = $"Відповідь з ID '{id}' успішно видалено." };
        }
    }
}