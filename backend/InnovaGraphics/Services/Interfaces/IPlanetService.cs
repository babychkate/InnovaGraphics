using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IPlanetService : IBaseService<Planet>
    {
        Task<Response> CreatePlanetAsync(CreatePlanetDto newPlanetDto);
        Task<IEnumerable<string>> GetAllPlanetTopicsAsync();
        Task<IEnumerable<string>> GetAllPlanetSubTopicsAsync();
        Task<IEnumerable<string>> GetAllPlanetNamesAsync();
    }
}
