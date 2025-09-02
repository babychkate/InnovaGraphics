using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using InnovaGraphics.Dtos;
using InnovaGraphics.Extensions;
using InnovaGraphics.Utils.Factory.Interfaces;
using Microsoft.Extensions.Options;

namespace InnovaGraphics.Utils.Factory.Implementations
{
    public class YouTubeMetadataFetcher : IMaterialMetadataFetcher
    {
        private readonly YouTubeService _youtubeService;

        public YouTubeMetadataFetcher(IOptions<YouTubeSettings> options)
        {
            var settings = options.Value;

            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = settings.ApiKey,
                ApplicationName = settings.ApplicationName
            });
        }

        public async Task<MaterialMetaDto> FetchAsync(string youtubeUrl)
        {
            string videoId = GetVideoIdFromUrl(youtubeUrl);

            if (videoId == null)
                return new MaterialMetaDto
                {
                    Name = null,
                    Description = "Invalid YouTube URL",
                    Link = null,
                    PhotoPath = null
                };


            var request = _youtubeService.Videos.List("snippet");
            request.Id = videoId;

            var response = await request.ExecuteAsync();
            if (response.Items.Count == 0)
                return new MaterialMetaDto
                {
                    Name = null,
                    Description = "YouTube video not found",
                    Link = null,
                    PhotoPath = null
                };


            var video = response.Items[0];

            return new MaterialMetaDto
            {
                Name = video.Snippet.Title,
                Description = video.Snippet.Description,
                Link = youtubeUrl,
                PhotoPath = video.Snippet.Thumbnails.High?.Url
            };
        }

        public string GetVideoIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);

                // Якщо посилання коротке youtu.be/VIDEOID
                if (uri.Host.Contains("youtu.be"))
                {
                    var id = uri.Segments.LastOrDefault()?.Trim('/');
                    if (!string.IsNullOrEmpty(id) && id.Length == 11)
                        return id;
                    return null;
                }

                // Якщо посилання стандартне youtube.com/watch?v=VIDEOID
                if (uri.Host.Contains("youtube.com") || uri.Host.Contains("youtube-nocookie.com"))
                {
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var v = query["v"];
                    if (!string.IsNullOrEmpty(v) && v.Length == 11)
                        return v;

                    // Іноді може бути формат /embed/VIDEOID або /v/VIDEOID
                    var segments = uri.Segments;
                    if (segments.Length >= 2)
                    {
                        var possibleId = segments[segments.Length - 1].Trim('/');
                        if (possibleId.Length == 11)
                            return possibleId;
                    }
                }

                return null;
            }
            catch
            {
                return null; // Якщо URL некоректний
            }
        }

    }
}