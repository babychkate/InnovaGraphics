using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> GetQuestionByIdAsync(Guid id);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Response> CreateQuestionAsync(CreateQuestionDto newQuestionDto, Guid testId);
        Task<Response> UpdateQuestionAsync(Guid id, JsonPatchDocument<Question> patchDoc);
        Task<Response> DeleteQuestionAsync(Guid id);
        Task<IEnumerable<Question>> GetQuestionsByTestIdAsync(Guid testId);
    }
}
