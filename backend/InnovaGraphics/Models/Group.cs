using System.Text.Json.Serialization;

namespace InnovaGraphics.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //1:N
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
