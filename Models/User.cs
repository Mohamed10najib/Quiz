using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]

        public ICollection<Quiz>? quizzes { get; set; }
        public ICollection<StartedQuizStudent>? StartedQuizStudents { get; set; }
        public ICollection<StartedQuizTeacher>? StartedQuizTeachers { get; set; }

    }
}
