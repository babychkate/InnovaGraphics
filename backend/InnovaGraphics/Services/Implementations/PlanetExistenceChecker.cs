using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Services.Implementations
{
    public class PlanetExistenceChecker : IExistenceChecker<Planet, Guid>
    {
        private readonly AppDbContext _dbContext;

        public PlanetExistenceChecker(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Set<Planet>().FindAsync(id) != null;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _dbContext.Set<Planet>().AnyAsync(p => p.Name == name);
        }
    }
}
