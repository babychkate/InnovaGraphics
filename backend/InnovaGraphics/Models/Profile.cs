namespace InnovaGraphics.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string? AboutMyself { get; set; }
        public string? InstagramLink { get; set; }
        public string? GitHubLink { get; set; }
        public string? LinkedInLink { get; set; }

        //1:1
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid? AvatarId { get; set; }
        public Avatar Avatar { get; set; }
        public Guid? BackgroundId { get; set; }
        public Background Background { get; set; }
        public Guid? ResourceId { get; set; }
        public Resource Resource { get; set; }
        public Guid? MusicThemeId { get; set; }
        public MusicTheme MusicTheme { get; set; }

    }
}
