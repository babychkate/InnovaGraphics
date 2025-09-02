using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> FindByTokenAsync(string token);
        Task<RefreshToken?> FindValidTokenByUserIdAsync(string userId);
    }
}
