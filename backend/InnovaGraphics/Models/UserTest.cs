namespace InnovaGraphics.Models
{
    public class UserTest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public bool IsCompleted { get; set; }
    }
}
