using Microsoft.EntityFrameworkCore;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Data;

namespace InnovaGraphics.Repositories.Implementations
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> FindByTokenAsync(string token)
        {
            return await _context.Set<RefreshToken>().SingleOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<RefreshToken?> FindValidTokenByUserIdAsync(string userId)
        {
            return await _context.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId && rt.Revoked == null && rt.Expires > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

    }
}