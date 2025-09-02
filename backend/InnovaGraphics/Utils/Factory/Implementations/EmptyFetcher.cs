using InnovaGraphics.Dtos;
using InnovaGraphics.Utils.Factory.Interfaces;

namespace InnovaGraphics.Utils.Factory.Implementations
{
    public class EmptyFetcher : IMaterialMetadataFetcher
    {
        public Task<MaterialMetaDto> FetchAsync(string url)
        {
            return Task.FromResult(new MaterialMetaDto());
        }
    }

}
