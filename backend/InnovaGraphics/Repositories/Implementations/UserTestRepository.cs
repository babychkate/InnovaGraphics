using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class UserTestRepository : BaseRepository<UserTest>, IUserTestRepository
    {
        public UserTestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> GetCompletedTestsCountAsync(string userId)
        {
            return await EntityFrameworkQueryableExtensions.CountAsync( 
                _context.Set<UserTest>(),
                ut => ut.UserId == userId && ut.IsCompleted
            );
        }
    }
}
