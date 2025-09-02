using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaGraphics.Models
{
    public class Battle
    {
        public Guid Id { get; set; }

        //1:N

        [ForeignKey("HostUser")] 
        public string HostUserId { get; set; }
        public User HostUser { get; set; }

        [ForeignKey("OpponentUser")] 
        public string OpponentUserId { get; set; }        
        public User OpponentUser { get; set; }

        //N:1
        public Guid TestId { get; set; }
        public Test Test { get; set; }        
        public List<CompetitionInfo> CompetitionInfos { get; set; }
    }
}
