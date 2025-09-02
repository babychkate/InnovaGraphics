namespace InnovaGraphics.Interactions
{
    public class RefreshTokenResponse : Response
    {
        public string AccessToken { get; set; }
        public string NewRefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}
