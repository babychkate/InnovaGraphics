using InnovaGraphics.Interactions;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<Response> UpdateAsync(Guid id, JsonPatchDocument<T> dto);
        Task<Response> DeleteAsync(Guid id);
    }
}
