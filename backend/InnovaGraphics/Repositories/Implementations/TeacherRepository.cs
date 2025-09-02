using InnovaGraphics.Data;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _dbContext;

        public TeacherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckIfTeacherExistsByEmailAsync(string email)
        {
            return await _dbContext.Teacher.AnyAsync(t => t.Email == email);
        }
    }
}
