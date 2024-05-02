using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class StartedQuizStudent
    {
        public StartedQuizStudent() { }
       public StartedQuizStudent(int?_UserId,int? _StartedQuizTeacherId)
        {

            UserId = _UserId;
            IdStartedQuizTeacher = _StartedQuizTeacherId;
            Score = 0;

        }
        [Key]
        public int Id { get; set; }


        [ForeignKey("UserIdl")]
        public int? UserId { get; set; }
        public User? UserStudent { get; set; }

        [ForeignKey("StartedQuizTeacherId")]
        public int? IdStartedQuizTeacher { get; set; }
        public StartedQuizTeacher? StartedQuizTeacher { get; set; }

        public int Score { get; set; }
    }
}
