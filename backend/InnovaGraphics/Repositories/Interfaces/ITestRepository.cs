using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface ITestRepository : IRepository<Test>
    {
        Task<Test> GetByNameAsync(string name);
        Task<Test?> GetByIdWithQuestionsAsync(Guid id);
        Task<Test?> GetByIdWithQuestionsAndAnswersAsync(Guid id);
        Task<IEnumerable<string>> GetAllTestNamesAsync();
        Task<Test?> GetBySubTopicAsync(string subTopic);
        Task<Test?> GetByPlanetIdAsync(Guid planetId);
        Task<Test> GetByIdWithUsersAsync(Guid id);

    }
}
