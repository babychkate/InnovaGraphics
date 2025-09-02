namespace InnovaGraphics.Models
{
    public class Certificate
    {
        public Guid Id { get; set; }
        public DateOnly Date {  get; set; }
        //1:1
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
