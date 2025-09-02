namespace InnovaGraphics.Dtos
{
    public class UserGetDto
    {
        public string Id { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? Group { get; set; }
        public bool IsTeacher { get; set; }
        public int CoinCount { get; set; }
        public int EnergyCount { get; set; }
        public int MarkCount { get; set; }
        public ProfileDto? Profile { get; set; }        
    }
}
