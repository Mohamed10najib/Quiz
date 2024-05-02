using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IStartedQuizRepository
    {
        bool AddStartedStudent(Models.StartedQuizStudent startedQuizStudent);
        bool AddStartedTeacher(Models.StartedQuizTeacher startedQuizTeacher);
        Task<StartedQuizTeacher> GetStartedQuizByCodeQuiz(string  codeQuiz);
    }
}
