using System.Text.Json.Serialization;

namespace API_Edunas.Models
{
    public class Quiz
    {
        public int ID { get; set; }
        public int Subject_id { get; set; }
        public string Title { get; set; }
        public int Total_questions { get; set; }

        [JsonIgnore]
        public Subjects? Subjects { get; set; }

        [JsonIgnore]
        public ICollection<Quiz_question>? Questions { get; set; }

        [JsonIgnore]
        public ICollection<User_quiz_result>? quiz_Results { get; set; }
    }
}