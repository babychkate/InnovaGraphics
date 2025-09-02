using InnovaGraphics.Models;
using System.Linq.Expressions;

namespace InnovaGraphics.Repositories.Interfaces
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<Certificate> GetAsync(Expression<Func<Certificate, bool>> predicate);
    }
}
