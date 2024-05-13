using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.interfaces;

namespace Quiz.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDBContext _context;

        public QuizRepository(ApplicationDBContext context)
        {
            this._context = context;
        }
        public bool Add(Models.Quiz quiz)
        {
            _context.Add(quiz);
            return Save();
        }
        public bool Delete(Models.Quiz quiz)
        {
            // Récupérer tous les StartedQuizTeacher associés au Quiz
            var startedQuizTeachers = _context.StartedQuizTeachers.Where(sq => sq.QuizId == quiz.QuizId).ToList();

            // Supprimer chaque StartedQuizTeacher associé au Quiz
            foreach (var startedQuizTeacher in startedQuizTeachers)
            {
                _context.Remove(startedQuizTeacher);
            }

            // Supprimer ensuite le Quiz
            _context.Remove(quiz);

            // Enregistrer les modifications
            return Save();
        }

        public async Task<IEnumerable<Models.Quiz>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }


        public async Task<List<Models.Quiz>> GetQuizzesByUserIdAsync(int userId)
        {
            return await _context.Quizzes
                .Where(q => q.UserId == userId)
                .ToListAsync();
        }
        public async Task<Models.Quiz> GetById(int idquiz)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions) // Include the questions related to the quiz
                .Include(q => q.user)      // Include the user related to the quiz
                .FirstOrDefaultAsync(q => q.QuizId == idquiz);

            // If the quiz is found, return it; otherwise, return null
            return quiz;
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Models.Quiz quiz)
        {
            _context.Update(quiz);
            return Save();
        }
    }
}