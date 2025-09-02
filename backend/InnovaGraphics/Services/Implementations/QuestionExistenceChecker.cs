using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;

namespace InnovaGraphics.Services.Implementations
{
    public class QuestionExistenceChecker : IExistenceChecker<Question, Guid>
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionExistenceChecker(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _questionRepository.GetByIdAsync(id) != null;
        }
        public async Task<bool> ExistsAsync(string name)
        {
            return await _questionRepository.GetByNameAsync(name) != null;
        }
    }
}
