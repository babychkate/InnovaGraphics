using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IQuestionRepository: IRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByTestIdAsync(Guid testId);
        
        //Функція для перевірки, чи не повторюються номери питань в межах тесту
        Task<Question> GetQuestionByTestIdAndNumberAsync(Guid testId, int number);
        Task<Question> GetByIdWithAnswersAsync(Guid id);
        Task<Question> GetByNameAsync(string name);
    }
}
