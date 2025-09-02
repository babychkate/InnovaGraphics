namespace InnovaGraphics.Models
{
    public class CompetitionInfo
    {
        public Guid Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Rival { get; set; }
        public bool IsWinner { get; set; }

        //N:1
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid BattleId { get; set; }
        public Battle Battle { get; set; }


    }
}
