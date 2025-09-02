using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface ICaseRepository : IRepository<Case>
    {
        Task<IEnumerable<Case>> GetByExerciseIdAsync(Guid exerciseId);
    }
}
