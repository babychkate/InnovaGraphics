using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<User> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _context = dbContext;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(string Id)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task UpdatePasswordHashAsync(User user, string hashedPassword)
        {
            user.PasswordHash = hashedPassword;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User?> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> HashPasswordAsync(User user, string password)
        {
            return await Task.FromResult(_userManager.PasswordHasher.HashPassword(user, password));
        }

        public async Task<IdentityResult> SetNewPasswordAsync(User user, string newPassword)
        {
            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);

            user.PasswordHash = hashedPassword;

            var stampResult = await _userManager.UpdateSecurityStampAsync(user);

            if (!stampResult.Succeeded)
            {
                return stampResult;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetTopUsersByMarkCountAsync(int count)
        {
            return await _context.Users
                .OrderByDescending(u => u.MarkCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<string> ids)
        {
            return await _context.Users
                .Where(user => ids.Contains(user.Id))
                .Include(u => u.Profile)
                .ToListAsync();
        }

        public async Task AddPlanetToUserAsync(string userId, Guid planetId)
        {
            var user = await _context.Users.Include(u => u.Planets).FirstOrDefaultAsync(u => u.Id == userId);
            var planet = await _context.Planet.FindAsync(planetId);

            if (user != null && planet != null)
            {
                user.Planets ??= new List<Planet>();
                if (!user.Planets.Contains(planet))
                {
                    user.Planets.Add(planet);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Planet>> GetUserPlanetsAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.Planets).FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Planets ?? new List<Planet>();
        }

        public async Task<User> GetByIdWithPlanetsAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Planets)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
