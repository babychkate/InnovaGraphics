using AngleSharp;
using InnovaGraphics.Dtos;
using InnovaGraphics.Utils.Factory.Interfaces;

namespace InnovaGraphics.Utils.Factory.Implementations
{
    public class HtmlMetadataFetcher : IMaterialMetadataFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string default_icon = "https://www.shutterstock.com/image-vector/gearsettings-arrows-icon-reset-button-600nw-1725398629.jpg";

        public HtmlMetadataFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MaterialMetaDto> FetchAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));
            var head = document.Head;

            if (head == null)
                return null;

            string GetMetaContent(string nameOrProperty)
            {
                var meta = head.QuerySelector($"meta[name='{nameOrProperty}']") ??
                           head.QuerySelector($"meta[property='{nameOrProperty}']");
                return meta?.GetAttribute("content");
            }

            // 4. Пошук favicon
            string favicon = head.QuerySelector("link[rel~='icon']")?.GetAttribute("href")
                          ?? head.QuerySelector("link[rel='shortcut icon']")?.GetAttribute("href")
                          ?? head.QuerySelector("link[rel='apple-touch-icon']")?.GetAttribute("href");

            if (favicon != null && !favicon.StartsWith("http"))
            {
                var uri = new Uri(url);
                favicon = new Uri(uri, favicon).ToString();
            }

            var metadata = new MaterialMetaDto
            {
                Name = document.Title,
                Description = GetMetaContent("description") ?? "Без опису",
                PhotoPath = favicon ?? default_icon
            };

            return metadata;
        }

    }
}