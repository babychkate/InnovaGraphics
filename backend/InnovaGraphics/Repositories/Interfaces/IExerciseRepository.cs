using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        Task<IEnumerable<Exercise>> GetByPlanetIdAsync(Guid planetId);
        Task<Exercise> GetByIdAsync(Guid id);
    }
}
