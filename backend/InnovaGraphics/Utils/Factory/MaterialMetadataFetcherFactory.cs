using InnovaGraphics.Enums;
using InnovaGraphics.Utils.Factory.Implementations;
using InnovaGraphics.Utils.Factory.Interfaces;

namespace InnovaGraphics.Utils.Factory
{
    public class MaterialMetadataFetcherFactory
    {
        private readonly YouTubeMetadataFetcher _youTubeFetcher;
        private readonly HtmlMetadataFetcher _htmlFetcher;

        public MaterialMetadataFetcherFactory(YouTubeMetadataFetcher youTubeFetcher, HtmlMetadataFetcher htmlFetcher)
        {
            _youTubeFetcher = youTubeFetcher;
            _htmlFetcher = htmlFetcher;
        }

        public IMaterialMetadataFetcher GetFetcher(TypeEnum type)
        {
            return type switch
            {
                TypeEnum.Video => _youTubeFetcher,
                TypeEnum.Site => _htmlFetcher,
                TypeEnum.Course => _htmlFetcher,
                _ => new EmptyFetcher()
            };
        }
    }

}
