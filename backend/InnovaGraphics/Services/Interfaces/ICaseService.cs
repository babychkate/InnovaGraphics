using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface ICaseService : IBaseService<Case>
    {
        Task<Response> CreateAsync(CreateCaseDto dto);
    }
}
