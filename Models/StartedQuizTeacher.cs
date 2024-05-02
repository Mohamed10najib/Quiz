using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class StartedQuizTeacher
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; } // Assuming User table has UserId as primary key
        public User Teacher { get; set; }

        [ForeignKey("Quiz")]
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public string CodeQuiz { get; set; }

        public bool IsStarted { get; set; }

        public bool IsTerminated { get; set; }

        // Navigation property to represent the list of students who have started this quiz
        public ICollection<StartedQuizStudent>? ListStudents { get; set; }
    }
}
