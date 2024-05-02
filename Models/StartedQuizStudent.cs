using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class StartedQuizStudent
    {
        [Key]
        public int Id { get; set; }


        public int? UserId { get; set; }
        public User? UserStudent { get; set; }


        public int? StartedQuizTeacherId { get; set; } // Explicitly specify the foreign key property name
        public StartedQuizTeacher? StartedQuizTeacher { get; set; }

        public int Score { get; set; }
    }
}
