using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Repositories;

namespace BackEnd.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<List<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _userRepository.GetUserByNameAsync(name);
        }

        public async Task PostUserAsync(User user)
        {
            await _userRepository.PostUserAsync(user);
        }

        public async Task<bool> PutUserAsync(User user)
        {
            return await _userRepository.PutUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> UserExistsAsync(long id)
        {
            return await _userRepository.UserExistsAsync(id);
        }
    }
}