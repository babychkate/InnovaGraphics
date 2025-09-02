using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {

        public QuestionRepository(AppDbContext context) : base(context)
        {
        }        

        public async Task<Question> GetByNameAsync(string name)
        {
            return await _context.Question.FirstOrDefaultAsync(q => q.Text.ToLower() == name.ToLower());
        }

        public async Task<Question> GetByIdWithAnswersAsync(Guid id)
        {
            return await _context.Question
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByTestIdAsync(Guid testId)
        {
            return await _context.Question
                .Where(q => q.TestId == testId)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByTestIdAndNumberAsync(Guid testId, int number)
        {
            return await _context.Question
                .Where(q => q.TestId == testId && q.Number == number)
                .FirstOrDefaultAsync();
        }
    }
}
