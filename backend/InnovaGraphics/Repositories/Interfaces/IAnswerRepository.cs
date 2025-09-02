using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId);
    }
}
