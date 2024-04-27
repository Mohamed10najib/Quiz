using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.interfaces;
using Quiz.Models;

namespace Quiz.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            this._context = context;
        }
        public bool Add(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool checkUser(User user)
        {
            return _context.Users.Any(u => u.Username == user.Username && u.Password == user.Password);
        }

        public bool Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }
        public async Task<User?> GetByPasswordAsync(String password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Password == password);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
