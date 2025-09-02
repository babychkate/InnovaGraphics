using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Repositories.Implementations
{
    public class AnswerRepository: BaseRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId)
        {
            return await _context.Answer
                           .Where(a => a.QuestionId == questionId)
                           .ToListAsync();
        }
    }
}
