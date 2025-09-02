using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface ITheoryRepository : IRepository<Theory>
    {
        Task<IEnumerable<Theory>> GetByPlanetIdAsync(Guid theoryId);
    }
}
