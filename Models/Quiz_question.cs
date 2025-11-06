namespace API_Edunas.Models
{
    public class Quiz_question
    {
        public int ID { get; set; }
        public int quiz_id { get; set; }
        public Quiz? Quiz { get; set; }
        public string question_text { get; set; }
        public string option_a {  get; set; }
        public string option_b { get; set; }
        public string option_c { get; set; }
        public string option_d { get; set; }
        public string correct_option { get; set; }
       
    }
}
