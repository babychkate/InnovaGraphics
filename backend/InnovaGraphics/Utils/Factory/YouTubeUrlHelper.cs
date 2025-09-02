namespace InnovaGraphics.Utils.Factory
{
    public static class YouTubeUrlHelper
    {
        public static string? ExtractVideoId(string url)
        {
            try
            {
                var uri = new Uri(url);

                if (uri.Host.Contains("youtu.be"))
                {
                    var id = uri.Segments.LastOrDefault()?.Trim('/');
                    if (!string.IsNullOrEmpty(id) && id.Length == 11)
                        return id;
                }

                if (uri.Host.Contains("youtube.com") || uri.Host.Contains("youtube-nocookie.com"))
                {
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var v = query["v"];
                    if (!string.IsNullOrEmpty(v) && v.Length == 11)
                        return v;

                    var segments = uri.Segments;
                    if (segments.Length >= 2)
                    {
                        var possibleId = segments[^1].Trim('/');
                        if (possibleId.Length == 11)
                            return possibleId;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
