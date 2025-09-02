using InnovaGraphics.Data;
using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using InnovaGraphics.Repositories.Interfaces.InnovaGraphics.Repositories.Interfaces; 

namespace InnovaGraphics.Repositories.Implementations
{
    public class TokenManagerRepository : BaseRepository<TokenManager>, ITokenManagerRepository
    {
        public TokenManagerRepository(AppDbContext context) : base(context) {}

        public async Task<TokenManager?> FindByTokenAsync(string token)
        {
            return await _context.Set<TokenManager>().FirstOrDefaultAsync(tm => tm.Token == token);
        }

        public async Task DeleteAsync(TokenManager tokenEntry)
        {
            if (_context.Entry(tokenEntry).State == EntityState.Detached)
            {
                _context.Set<TokenManager>().Attach(tokenEntry);
            }
            _context.Set<TokenManager>().Remove(tokenEntry);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteExpiredTokensAsync(DateTimeOffset cutoffTime)
        {
            var expiredTokens = await _context.Set<TokenManager>()
                .Where(tm => tm.Expires <= cutoffTime)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.Set<TokenManager>().RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }        
    }
}