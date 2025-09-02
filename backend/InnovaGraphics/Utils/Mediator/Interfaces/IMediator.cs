using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;

namespace InnovaGraphics.Utils.Mediator.Interfaces
{
    public interface IMediator
    {
        // Розділяє залежність TestService від Question та Answer репозиторіїв
        Task<Response> CreateQuestionAsync(CreateQuestionDto newQuestionDto, Guid testId);
        Task<Response> CreateAnswerAsync(CreateAnswerDto newAnswerDto, Guid questionId);
    }
}
