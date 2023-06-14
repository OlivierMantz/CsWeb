using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            _users = new List<User>
            {
                new User { Id = 1, Name = "John", Email = "John@gmail.com", Password = "1234", Role = "User", IsBanned = false },
                new User { Id = 2, Name = "Jane", Email = "Jane@gmail.com", Password = "1234", Role = "User", IsBanned = false },
                new User { Id = 3, Name = "Bob", Email = "Bob@gmail.com", Password = "1234", Role = "User", IsBanned = false }
            };
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await Task.FromResult(_users.FirstOrDefault(user => user.Id == id));
        }
        public async Task<User> GetUserByNameAsync(string name)
        {
            return await Task.FromResult(_users.FirstOrDefault(user => user.Name == name));
        }

        public async Task PostUserAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }
        public async Task<bool> PutUserAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
                return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;
            existingUser.IsBanned = user.IsBanned;

            return true;
        }
        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        
        public async Task<bool> UserExistsAsync(long userId)
        {
            return _users.Any(u => u.Id == userId);
        }
    }
}
