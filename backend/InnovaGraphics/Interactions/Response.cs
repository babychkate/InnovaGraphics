namespace InnovaGraphics.Interactions
{
    public class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty; 
        public object? Data { get; set; }
        public object? AdditionalData { get; set; }
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }
    }
}
