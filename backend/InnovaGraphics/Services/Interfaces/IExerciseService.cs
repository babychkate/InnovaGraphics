using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<Exercise> GetExerciseByIdAsync(Guid id);
    }
}
