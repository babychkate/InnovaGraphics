using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class TheoryRepository : BaseRepository<Theory>, ITheoryRepository
    {
        public TheoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Theory>> GetByPlanetIdAsync(Guid planetId)
        {
            return await _context.Set<Theory>()
                                 .Where(c => c.PlanetId == planetId)
                                 .ToListAsync();
        }
    }
}
