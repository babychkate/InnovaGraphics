namespace InnovaGraphics.Dtos
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string? AboutMyself { get; set; }
        public string? InstagramLink { get; set; }
        public string? GitHubLink { get; set; }
        public string? LinkedInLink { get; set; }
        public Guid? AvatarId { get; set; }
        public Guid? BackgroundId { get; set; }
        public Guid? ResourceId { get; set; }
    }
}
