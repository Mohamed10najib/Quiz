using Microsoft.EntityFrameworkCore;
using Quiz.Models;
using System.Diagnostics;
using System.Net;

namespace Quiz.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Scores> Scores { get; set; }

        public DbSet<StartedQuizTeacher> StartedQuizTeachers { get; set; }
        public DbSet<StartedQuizStudent> StartedQuizStudents { get; set; }
    }
}
