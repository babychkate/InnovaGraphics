using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Exercise>> GetByPlanetIdAsync(Guid planetId)
        {
            return await _context.Set<Exercise>()
                                 .Where(c => c.PlanetId == planetId)
                                 .ToListAsync();
        }

        public async Task<Exercise> GetByIdAsync(Guid id)
        {
            return await _context.Exercise
                .FindAsync(id); 
        }
    }
}
