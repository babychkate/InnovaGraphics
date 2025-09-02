using InnovaGraphics.Models;

namespace InnovaGraphics.Repositories.Interfaces

{
    namespace InnovaGraphics.Repositories.Interfaces
    {
        public interface ITokenManagerRepository: IRepository<TokenManager>
        {
            Task<TokenManager?> FindByTokenAsync(string token);
            Task DeleteAsync(TokenManager tokenEntry);
            Task DeleteExpiredTokensAsync(DateTimeOffset cutoffTime);
        }
    }
}
