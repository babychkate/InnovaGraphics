namespace InnovaGraphics.Models
{
    public class Test
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public TimeOnly TimeLimit { get; set; }
        public int TestResult { get; set; }
        public bool IsQuickTest { get; set; }        
        public bool IsCompleted { get; set; }

        //N:1
        public Guid? PlanetId { get; set; }
        public Planet Planet { get; set; }

        //1:N
        public List<Question> Questions { get; set; }
        public List<Battle> Battles { get; set; }
        public List<UserTest> UserTests { get; set; }

        //M:N
        public List<User> Users { get; set; }
    }
}
