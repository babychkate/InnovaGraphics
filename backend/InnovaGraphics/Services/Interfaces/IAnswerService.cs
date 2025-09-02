using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> GetAnswerByIdAsync(Guid id);
        Task<IEnumerable<Answer>> GetAllAnswersAsync();
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId);
        Task<Response> CreateAnswerAsync(Guid questionId, CreateAnswerDto newAnswerDto);
        Task<Response> UpdateAnswerAsync(Guid id, JsonPatchDocument<Answer> patchDoc);
        Task<Response> DeleteAnswerAsync(Guid id);
    }
}
