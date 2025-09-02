using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;

namespace InnovaGraphics.Repositories.Implementations
{
    public class HintRepository : BaseRepository<Hint>, IHintRepository
    {
        public HintRepository(AppDbContext context) : base(context)
        {
        }
    }
}
