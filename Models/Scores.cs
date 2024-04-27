using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class Scores
    {
        [Key]
        public int Id { get; set; }
        public int score { get; set; }
        [ForeignKey("user")]
        public int UserId { get; set; }
        public User user { get; set; }

        [ForeignKey("quiz")]
        public int QuizId { get; set; }
        
        public Quiz quiz { get; set; }


    }
}
