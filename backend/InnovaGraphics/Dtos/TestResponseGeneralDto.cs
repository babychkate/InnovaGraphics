namespace InnovaGraphics.Dtos
{
    public class TestResponseGeneralDto
    {
        // Основна інформація про тест
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public TimeOnly TimeLimit { get; set; }
        public int TestResult { get; set; }
        public bool IsQuickTest { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? PlanetId { get; set; }
        public object? AdditionalData { get; set; }

        // Опціональні питання та відповіді
        public ICollection<QuestionGeneralDto>? Questions { get; set; }

        // Опціональні результати тесту
        public TestResultDto? TestResultDetails { get; set; }

        // Інформація про результат операції
        public string? Message { get; set; }
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }
    }
}
