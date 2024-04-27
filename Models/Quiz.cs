using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        public string? QuizName { get; set; }

        public string Description { get; set; }

        public int? DurationMinutes { get; set; }

        public int? NbrQuestion { get; set; }  

        public int? UserId { get; set; }
        public User? user { get; set; }
        public ICollection<Question>? Questions { get; set; }


    }
}
