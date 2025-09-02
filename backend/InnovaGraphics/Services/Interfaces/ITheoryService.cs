using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface ITheoryService : IBaseService<Theory>
    {
        Task<Response> CreateAsync(CreateTheoryDto dto);
    }
}
