
namespace InnovaGraphics.Services.Interfaces
{
    public interface IUserPlanetService
    {
        Task<bool> ConnectUserToPlanetAfterExerciseAsync(string userId, Guid exerciseId);
        Task<int> GetUserPlanetCountAsync(string userId);
    }
}