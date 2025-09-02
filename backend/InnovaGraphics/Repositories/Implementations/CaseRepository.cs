using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        public CaseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Case>> GetByExerciseIdAsync(Guid exerciseId)
        {
            return await _context.Set<Case>()
                                 .Where(c => c.ExerciseId == exerciseId)
                                 .ToListAsync();
        }

    }
}
