namespace API_Edunas.Models
{
    public class User_quiz_result
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int quiz_id { get; set; }
        public int score { get; set; }
        public int total_correct { get; set; }
        public int total_wrong { get; set; }
        public DateTime submitted_at { get; set; } = DateTime.Now;
        public Quiz Quiz{ get; set; }
    }
}
