using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IStartedQuizRepository
    {
        bool AddStartedStudent(Models.StartedQuizStudent startedQuizStudent);
        bool AddStartedTeacher(Models.StartedQuizTeacher startedQuizTeacher);
        Task<StartedQuizStudent> GetStartedQuizStudentAsync(int userId, int startedQuizTeacherId);
        Task<StartedQuizTeacher> GetStartedQuizByCodeQuiz(string  codeQuiz);
        Task<bool> IsExistStartedQuizByCodeQuiz(string codeQuiz);
        Task<StartedQuizStudent> GetStartedQuizStudentAsyncById(int id);
        Task<List<StartedQuizTeacher>> GetListStartedTeacher(int id);
        Task DeleteStartedQuizTeacherAsync(int id);

        Task<bool> IsJoinStudent(int studentid ,string codeQuiz);
        Task<IEnumerable<StartedQuizStudent>> ListStudentQuiz(int idStartedTeacher);
        bool Save();
        public void UpdateStartedQuizTeacher(StartedQuizTeacher startedQuizTeacher);
      
    }
}
