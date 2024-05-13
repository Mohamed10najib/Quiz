using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.interfaces;
using Quiz.Models;

namespace Quiz.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDBContext _context;
        public QuestionRepository(ApplicationDBContext context)
        {
            this._context = context;

        }
        public bool Add(Question question)
        {
            _context.Add(question);
            return Save();
        }

        public bool AddAll(List<Question> questions)
        {
            _context.Questions.AddRange(questions);
            return Save();
        }

        public bool Delete(Question question)
        {
            _context.Remove(question);
            return Save();
        }

        public async Task<IEnumerable<Question>> GetAll()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<List<Question>> GetQuestionByQuizzIdAsync(int id)
        {
            return await _context.Questions
                           .Where(q => q.QuizId == id)
                           .ToListAsync();
        }

        public bool Save()
        {

            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Question question)
        {
            _context.Update(question);
            return Save();
        }
    }
}