using Quiz.Models;

namespace Quiz.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByPasswordAsync(String password);

        bool checkUser(User user);

        bool Add(User user);

        bool Update(User user);

        bool Delete(User user);

        bool Save();
    }
}
