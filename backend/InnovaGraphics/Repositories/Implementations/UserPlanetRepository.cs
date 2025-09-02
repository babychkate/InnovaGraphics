using InnovaGraphics.Data;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InnovaGraphics.Repositories.Implementations
{
    public class UserPlanetRepository : IUserPlanetRepository
    {
        protected readonly AppDbContext _context;

        public UserPlanetRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<int> GetUserPlanetCountAsync(string userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Planets)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                return user.Planets.Count;
            }

            return 0;
        }
    }
}