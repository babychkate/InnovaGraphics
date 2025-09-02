using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Interfaces
{
    public interface ITestService : IService<Test>
    {
        Task<TestResponseGeneralDto> CreateTestAsync(CreateTestDto newTestDto);
        Task<TestResponseGeneralDto> StartTestAsync(string userEmail, Guid testId);
        Task<TestResponseGeneralDto> CompleteTestAsync(string userEmail, Guid testId, Dictionary<Guid, Guid?> userAnswers, string endTime);
        Task<TestResponseGeneralDto> CalculateTestScoreAsync(Guid testId, Dictionary<Guid, Guid?> userAnswers);
        Task<TestResponseGeneralDto> AddQuestionsWithAnswersToTestAsync(List<CreateQuestionWithAnswersDto> newQuestionsWithAnswersList, Guid testId);
        Task<TestResponseGeneralDto> UpdateTestAsync(Guid id, JsonPatchDocument<Test> patchDoc);
        Task<TestResponseGeneralDto?> GetTestByIdWithQuestionsAsync(Guid id);
        Task<Test?> GetTestByIdWithQuestionsAndAnswersAsync(Guid id);
        Task<IEnumerable<string>> GetAllTestNamesAsync();
        Task<TestResponseGeneralDto?> GetTestBySubTopicAsync(string subTopic);
        Task<TestResponseGeneralDto> GetTestByPlanetIdAsync(Guid planetId);
        Task<TestResponseGeneralDto> DeleteTest(Guid id);
        Task<Test> GetByIdAsync(Guid id);
        Task<IEnumerable<Test>> GetAllAsync(); 
    }
}
