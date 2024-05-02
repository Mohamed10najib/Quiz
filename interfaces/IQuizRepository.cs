using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Models.Quiz>> GetAll();
        Task<Models.Quiz> GetById(int idquiz);

        Task<List<Models.Quiz>> GetQuizzesByUserIdAsync(int id);

        bool Add(Models.Quiz quiz);

        bool Update(Models.Quiz quiz);

        bool Delete(Models.Quiz quiz);

        bool Save();
    }
}
