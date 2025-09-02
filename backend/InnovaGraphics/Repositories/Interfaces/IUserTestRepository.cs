using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IUserTestRepository : IRepository<UserTest>
    {
        Task<int> GetCompletedTestsCountAsync(string userId);
    }
}
