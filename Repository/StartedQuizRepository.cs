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
       public bool AddStartedStudent(Models.StartedQuizStudent startedQuizStudent)
        {
            _context.StartedQuizStudents.Add(startedQuizStudent);
           return  Save();



        }
        public bool AddStartedTeacher(Models.StartedQuizTeacher startedQuizTeacher)
        {
            try
            {
                _context.StartedQuizTeachers.Add(startedQuizTeacher);
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
    }
}
