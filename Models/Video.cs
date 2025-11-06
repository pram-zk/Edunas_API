using System.Text.Json.Serialization;

namespace API_Edunas.Models
{
    public class Video
    {
        public int ID { get; set; }
        public int Subject_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Video_url { get; set; }

        public Subjects? Subjects { get; set; }
    }
}
