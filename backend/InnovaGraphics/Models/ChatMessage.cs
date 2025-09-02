namespace InnovaGraphics.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string MessageText { get; set; }
        public TimeOnly Timestamp { get; set; }

        //M:N
        public User Sender { get; set; }
        public List<User> Recipients { get; set; }
    }
}
