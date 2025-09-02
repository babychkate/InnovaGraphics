namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IUserPlanetRepository
    {
        Task<int> GetUserPlanetCountAsync(string userId);
    }
}
