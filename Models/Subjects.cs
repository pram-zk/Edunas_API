using System.Text.Json.Serialization;

namespace API_Edunas.Models
{
    public class Subjects
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<Quiz> Quiz { get; set; }

        [JsonIgnore]
        public ICollection<Video>  Video { get; set; }
    }
}
