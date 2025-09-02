using InnovaGraphics.Data;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnovaGraphics.Repositories.Implementations
{
    public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(AppDbContext context) : base(context)
        {
        }

        public virtual async Task<Certificate> GetAsync(Expression<Func<Certificate, bool>> predicate)
        {
            return await _context.Set<Certificate>().FirstOrDefaultAsync(predicate);
        }
    }
}