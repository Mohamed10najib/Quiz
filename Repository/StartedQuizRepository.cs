using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.interfaces;
using Quiz.Models;

namespace Quiz.Repository
{
    public class StartedQuizRepository:IStartedQuizRepository
    {
        private readonly ApplicationDBContext _context;
        public StartedQuizRepository(ApplicationDBContext context)
        {
            this._context = context;
        }
        public async Task<bool> IsExistStartedQuizByCodeQuiz(string codeQuiz)
        {
            return await _context.StartedQuizTeachers.AnyAsync(sq => sq.CodeQuiz == codeQuiz);
        }
        public async Task<bool> IsJoinStudent(int idstudent, string codeQuiz)
        {
            return await _context.StartedQuizStudents
                .AnyAsync(sqs => sqs.UserId == idstudent && sqs.StartedQuizTeacher.CodeQuiz == codeQuiz);
        }
        public bool AddStartedStudent(Models.StartedQuizStudent startedQuizStudent)
        {
            _context.Add(startedQuizStudent);
           return  Save();



        }
        public async Task<StartedQuizTeacher> GetStartedQuizByCodeQuiz(string codeQuiz)
        {
          
            return await _context.StartedQuizTeachers
                .Include(sqt => sqt.Teacher) 
                .Include(sqt => sqt.Quiz) 
                .FirstOrDefaultAsync(sqt => sqt.CodeQuiz == codeQuiz);
        }

        public async Task<StartedQuizStudent> GetStartedQuizStudentAsync(int userId, int startedQuizTeacherId)
        {
            return await _context.StartedQuizStudents
            .Where(s => s.UserId == userId && s.IdStartedQuizTeacher == startedQuizTeacherId)
            .FirstOrDefaultAsync();
        }
        public async Task<StartedQuizStudent> GetStartedQuizStudentAsyncById(int id)
        {

            return await _context.StartedQuizStudents.FindAsync(id);
        }

        public bool AddStartedTeacher(Models.StartedQuizTeacher startedQuizTeacher)
        {
            try
            {
                _context.Add(startedQuizTeacher);
                return Save(); // Call Save method to save changes to the database
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false; // Return false to indicate failure
            }
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public void UpdateStartedQuizTeacher(StartedQuizTeacher startedQuizTeacher)
        {
            _context.StartedQuizTeachers.Update(startedQuizTeacher);
            Save();
        }
       public async Task<IEnumerable<StartedQuizStudent>> ListStudentQuiz(int idStartedTeacher)
        {

            var students = await _context.StartedQuizStudents
                   .Where(s => s.IdStartedQuizTeacher == idStartedTeacher)
                   .ToListAsync();

            return students;

        }

    }
}
