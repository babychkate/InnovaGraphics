using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace InnovaGraphics.Models
{
    public class User: IdentityUser
    {
        public string Role { get; set; }
        public string RealName { get; set; }
        public bool IsActive { get; set; }
        public int CoinCount { get; set; }
        public int EnergyCount {  get; set; }
        public int MarkCount { get; set; }

        //1:1       
        public Certificate Certificate { get; set; }
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public Profile Profile { get; set; }

        //1:N
        public List<CompetitionInfo> CompetitionInfos { get; set; }
        public List<Purchase> Purchases { get; set; }
        public List<UserTest> UserTests { get; set; }

        //N:1
        public int? GroupId { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }

        //N:M
        public List<Planet> Planets { get; set; }
        public List<Material> Materials { get; set; }
        public List<Test> Tests { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<Hint> Hints { get; set; }
        public List<ChatMessage> SentMessages { get; set; }
        public List<ChatMessage> ReceiveMessages { get; set; }
        public List<Battle> HostBattles { get; set; }
        public List<Battle> OpponentBattles { get; set; }

    }
}
