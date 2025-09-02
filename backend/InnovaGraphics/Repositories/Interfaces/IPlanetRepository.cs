using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IPlanetRepository : IRepository<Planet>
    {
        Task<Planet?> GetByNameAsync(string name);
        Task<Planet?> GetByNumberAsync(int number);

        Task<IEnumerable<string>> GetAllPlanetTopicsAsync();
        Task<IEnumerable<string>> GetAllPlanetSubTopicsAsync();
        Task<IEnumerable<string>> GetAllPlanetNamesAsync();

    }
}
