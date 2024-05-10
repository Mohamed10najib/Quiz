using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class StartedQuizTeacher
    {

        public StartedQuizTeacher()
        {
          
        }

        // Parameterized constructor
        public StartedQuizTeacher(int teacherId, int? quizId, string codeQuiz)
        {
            TeacherId = teacherId;
            QuizId = quizId;
            CodeQuiz = codeQuiz;
            IsStarted = false;
            DateCreation = DateTime.Now;
            IsTerminated = false;

        }
        [Key]
        public int IdStartedQuizTeacher { get; set; }

        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; } // Assuming User table has UserId as primary key
        public User Teacher { get; set; }

        [ForeignKey("Quiz")]
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public string CodeQuiz { get; set; }

        public bool IsStarted { get; set; }

        public bool IsTerminated { get; set; }
        public DateTime DateCreation { get; set; }
        // Navigation property to represent the list of students who have started this quiz
        public ICollection<StartedQuizStudent>? StartedQuizStudents { get; set; }
    }
}
