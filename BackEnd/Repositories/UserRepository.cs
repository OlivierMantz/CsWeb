using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userContext.User.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userContext.User.FindAsync(id);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _userContext.User.FirstOrDefaultAsync(user => user.Name == name);
        }

        public async Task PostUserAsync(User user)
        {
            await _userContext.User.AddAsync(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task<bool> PutUserAsync(User user)
        {
            _userContext.User.Update(user);
            var updated = await _userContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await _userContext.User.FindAsync(id);
            if (user != null)
            {
                _userContext.User.Remove(user);
                var deleted = await _userContext.SaveChangesAsync();
                return deleted > 0;
            }

            return false;
        }

        public async Task<bool> UserExistsAsync(long userId)
        {
            return await _userContext.User.AnyAsync(u => u.Id == userId);
        }
    }
}