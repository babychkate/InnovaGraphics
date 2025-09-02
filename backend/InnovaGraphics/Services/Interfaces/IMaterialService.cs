using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IMaterialService: IBaseService<Material>
    {
        Task<MaterialResponseGeneral> CreateAsync(CreateMaterialDto dto);
        Task<Response> GetAllMaterialsTypes();
        Task<Response> GetAllMaterialsThemes();
    }
}
