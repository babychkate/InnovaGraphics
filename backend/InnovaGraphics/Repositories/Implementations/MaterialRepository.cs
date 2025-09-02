using InnovaGraphics.Data;
using InnovaGraphics.Enums;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class MaterialRepository: BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Material> GetByNameAsync(string name)
        {
            return await _context.Material
            .FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task<IEnumerable<Material>> GetByTypeAsync(TypeEnum type)
        {
            return await _context.Material
                .Where(m => m.Type == type)
                .ToListAsync();
        }

    }
}
