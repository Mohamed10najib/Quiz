using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Models.Question>> GetAll();

        Task<List<Models.Question>> GetQuestionByQuizzIdAsync(int id);

        bool Add(Question question);
        bool AddAll(List<Question> questions);


        bool Update(Question question);

        bool Delete(Question question);

        bool Save();
    }
}
