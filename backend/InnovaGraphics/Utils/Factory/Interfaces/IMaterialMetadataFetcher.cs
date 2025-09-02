using InnovaGraphics.Dtos;

namespace InnovaGraphics.Utils.Factory.Interfaces
{
    public interface IMaterialMetadataFetcher
    {
        Task<MaterialMetaDto> FetchAsync(string link);
    }

}
