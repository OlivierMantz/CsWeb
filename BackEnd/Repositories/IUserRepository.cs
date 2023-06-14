using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByNameAsync(string name);
        Task PostUserAsync(User user);
        Task<bool> PutUserAsync(User user);
        Task<bool> DeleteUserAsync(long id);
        // Helper method
        Task<bool> UserExistsAsync(long id);
    }

}
