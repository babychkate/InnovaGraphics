using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {

        public TestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Test?> GetByNameAsync(string name)
        {
         return _context.Test.FirstOrDefault(t  => t.Name == name);
        }

        public async Task<IEnumerable<string>> GetAllTestNamesAsync()
        {
            return await _context.Test
                .Select(t => t.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();
        }

        public async Task<Test?> GetByIdWithQuestionsAsync(Guid id)
        {
            return await _context.Test
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Test?> GetByIdWithQuestionsAndAnswersAsync(Guid id)
        {
            return await _context.Test
                .Where(t => t.Id == id)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(t => t.Users) 
                .FirstOrDefaultAsync();
        }

        public async Task<Test?> GetBySubTopicAsync(string subTopic)
        {
            return await _context.Test
                                   .Include(t => t.Planet) // Включаємо пов'язану Planet
                                   .Include(t => t.Questions) // Включаємо питання
                                       .ThenInclude(q => q.Answers) // І відповіді до питань
                                   .FirstOrDefaultAsync(t => t.Planet != null && t.Planet.SubTopic == subTopic);
        }

        public async Task<Test> GetByIdWithUsersAsync(Guid id)
        {
            return await _context.Test
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<Test?> GetByPlanetIdAsync(Guid planetId)
        {
            return await _context.Test.FirstOrDefaultAsync(t => t.PlanetId == planetId);
        }
    }
}
