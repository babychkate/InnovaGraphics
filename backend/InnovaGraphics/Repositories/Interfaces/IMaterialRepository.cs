using InnovaGraphics.Enums;
using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IEnumerable<Material>> GetByTypeAsync(TypeEnum type);
        Task<Material> GetByNameAsync(string name);
    }
}
