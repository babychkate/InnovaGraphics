using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class PlanetRepository : BaseRepository<Planet>, IPlanetRepository
    {
        public PlanetRepository(AppDbContext context):  base(context)
        {
        }

        public async Task<Planet?> GetByNameAsync(string name)
        {
            return await _context.Planet.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Planet?> GetByNumberAsync(int number)
        {
            return await _context.Planet.FirstOrDefaultAsync(p => p.Number == number);
        }

        public async Task<IEnumerable<string>> GetAllPlanetTopicsAsync()
        {
            return await _context.Planet
                .Select(p => p.Topic)
                .Distinct()
                .OrderBy(theme => theme)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllPlanetSubTopicsAsync()
        {
            return await _context.Planet
              .Select(p => p.SubTopic)
              .Distinct()
              .OrderBy(subTopic => subTopic)
              .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllPlanetNamesAsync()
        {
            return await _context.Planet
               .Select(p => p.Name)
               .Distinct()
               .OrderBy(name => name)
               .ToListAsync();
        }
    }
}
