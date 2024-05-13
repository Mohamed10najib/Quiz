using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; }
        public int QuizId { get; set; }
        public  Quiz? quiz { get; set; }
        [Required]
        public string Response { get; set; }

        [Required]
        public string suggestion1 { get; set; }

        [Required]
        public string suggestion2 { get; set; }

        [Required]
        public string suggestion3 { get; set; }
        [Required]
        public string suggestion4 { get; set; }


    }
}
