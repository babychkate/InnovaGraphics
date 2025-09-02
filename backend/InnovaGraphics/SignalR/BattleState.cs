namespace InnovaGraphics.SignalR
{
    public class BattleState
    {
        public string User1 { get; set; }
        public string User2 { get; set; }
        public bool User1Completed { get; set; } = false;
        public bool User2Completed { get; set; } = false;
        public double? User1CompletionTime { get; set; }
        public double? User2CompletionTime { get; set; }
        public BattleStatus Status { get; set; }
    }

    public enum BattleStatus
    {
        InProgress,
        Completed
    }
}
