namespace InnovaGraphics.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Email { get; set; }

        //1:1
        public User User { get; set; }
    }
}
