namespace InnovaGraphics.Dtos
{
    public class CompleteTestRequestDto
    {
        public string UserEmail { get; set; }
        public Dictionary<Guid, Guid?> UserAnswers { get; set; }
        public string EndTime { get; set; }
    }
}
