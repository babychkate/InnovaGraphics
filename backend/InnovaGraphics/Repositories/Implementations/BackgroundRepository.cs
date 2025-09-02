using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Implementations;

namespace InnovaGraphics.Repositories.Implementations
{
    public class BackgroundRepository : BaseRepository<Background>, IBackgroundRepository
    {
        public BackgroundRepository(AppDbContext context) : base(context)
        {
        }
    }
}
