namespace InnovaGraphics.Dtos
{
    public class RegisterDto
    {
        public string RealName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? Group {  get; set; }
        public string Password { get; set; }
        public bool IsTeacher { get; set; }  
    }
}
