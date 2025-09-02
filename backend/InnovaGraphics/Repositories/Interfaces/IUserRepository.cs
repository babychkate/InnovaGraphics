using InnovaGraphics.Models;
using Microsoft.AspNetCore.Identity;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface IUserRepository 
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task<bool> CheckPasswordAsync(User user, string password); 
        Task UpdatePasswordHashAsync(User user, string newPasswordHash);
        Task UpdateAsync(User user);

        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string roleName);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByNameAsync(string userName);
        Task<string> HashPasswordAsync(User user, string password);


        Task<IdentityResult> SetNewPasswordAsync(User user, string newPassword);
        Task<IEnumerable<User>> GetTopUsersByMarkCountAsync(int count);

        Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<string> ids);

        Task AddPlanetToUserAsync(string userId, Guid planetId);
        Task<IEnumerable<Planet>> GetUserPlanetsAsync(string userId);
        Task<User> GetByIdWithPlanetsAsync(string id);
    }
}
