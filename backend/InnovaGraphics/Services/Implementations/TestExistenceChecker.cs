using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;

namespace InnovaGraphics.Services.Implementations
{
    public class TestExistenceChecker : IExistenceChecker<Test, Guid>
    {
        private readonly ITestRepository _testRepository;

        public TestExistenceChecker(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _testRepository.GetByIdAsync(id) != null;
        }
        public async Task<bool> ExistsAsync(string name)
        {
            return await _testRepository.GetByNameAsync(name) != null;
        }
    }
}
