using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IUserService
    {
        //Task<User> GetByEmailAsync(string email);
        //Task<User> GetByIdAsync(int id);
        //Task<bool> CheckPasswordAsync(User user, string password); 
        //Task UpdatePasswordHashAsync(User user, string newPasswordHash);
        //Task<IdentityResult> CreateUserWithPasswordAsync(User user, string password);

        Task<Response> UpdateUserAsync(string userId, JsonPatchDocument<User> patchDocument);
        Task<Response> AddMarksAndCoinsAsync(string userId, int mark, int coin);
    }
}
