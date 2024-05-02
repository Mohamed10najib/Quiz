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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between StartedQuizTeacher and StartedQuizStudent
            modelBuilder.Entity<StartedQuizStudent>()
                .HasOne(sqs => sqs.StartedQuizTeacher) // Each StartedQuizStudent has one StartedQuizTeacher
                .WithMany(sq => sq.StartedQuizStudents)       // Each StartedQuizTeacher can have many StartedQuizStudents
                .HasForeignKey(sqs => sqs.IdStartedQuizTeacher) // Foreign key property
                .IsRequired(); // Assuming the relationship is required

            // Optionally, configure cascading delete behavior if needed
            modelBuilder.Entity<StartedQuizStudent>()
                .HasOne(sqs => sqs.StartedQuizTeacher)
                .WithMany(sq => sq.StartedQuizStudents)
                .HasForeignKey(sqs => sqs.IdStartedQuizTeacher)
                .OnDelete(DeleteBehavior.Cascade); // Cascading delete behavior
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Scores> Scores { get; set; }

        public DbSet<StartedQuizTeacher> StartedQuizTeachers { get; set; }
        public DbSet<StartedQuizStudent> StartedQuizStudents { get; set; }
    }
}
