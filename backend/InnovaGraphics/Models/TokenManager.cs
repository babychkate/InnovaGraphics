namespace InnovaGraphics.Models
{
    public class TokenManager
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
