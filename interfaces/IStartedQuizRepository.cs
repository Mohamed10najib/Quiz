using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IStartedQuizRepository
    {
        bool AddStartedStudent(Models.StartedQuizStudent startedQuizStudent);
        bool AddStartedTeacher(Models.StartedQuizTeacher startedQuizTeacher);
        Task<StartedQuizTeacher> GetStartedQuizByCodeQuiz(string  codeQuiz);
        Task<bool> IsExistStartedQuizByCodeQuiz(string codeQuiz);
        Task<bool> IsJoinStudent(int studentid ,string codeQuiz);
        Task<IEnumerable<StartedQuizStudent>> ListStudentQuiz(int idStartedTeacher);
        bool Save();
        public void UpdateStartedQuizTeacher(StartedQuizTeacher startedQuizTeacher);
      
    }
}
